using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Unity.Mathematics;
using UnityEngine;

public class WeaponRaycastHandler : MonoBehaviour
{
    // The code for the weapon firing and raycast logic is referenced and modified from online tutorials
    // Author: TheKiwiCoder
    // Link: https://www.youtube.com/watch?v=onpteKMsE84
    class Bullet
    {
        public float time;
        public Vector3 initialPos;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    
    public bool isFiring = false;
    public int fireRate = 8;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0f;
    
    private float acculatedTime;
    public ParticleSystem muzzleFlashParticle;
    public ParticleSystem bulletHitParticlePrimary;
    public ParticleSystem bulletHitParticleSecondary;
    private ParticleSystem bulletHitParticle;
    public TrailRenderer bulletTracer;
    public Transform raycastOrigin;
    public GameObject missilePrefab;
    
    private Ray ray;
    private RaycastHit hitData;
    public Transform raycastDestination;
    private List<Bullet> bullets = new List<Bullet>();
    private float maxLifeTime = 3f;

    private int weaponMode = 0;
    
    public WeaponRecoil weaponRecoil;

    private PlayerStats playerStats;

    private void Awake()
    {
        weaponRecoil = GetComponent<WeaponRecoil>();
    }

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        bulletHitParticle = bulletHitParticlePrimary;
    }

    Vector3 GetBulletPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPos + bullet.initialVelocity * bullet.time + 0.5f * gravity * bullet.time * bullet.time;
    }

    Bullet SpawmBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPos = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(bulletTracer, position, quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }
    
    public void StartFiring()
    {
        isFiring = true;
        acculatedTime = 0f;
        //Fire();
    }

    public void UpdateFiringSequence(float time)
    {
        acculatedTime += time;
        float fireInterval = 1f / fireRate;
        while (acculatedTime >= 0f)
        {
            Fire();
            acculatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float time)
    {
        SimulateBullets(time);
        DestroyBullets();
    }

    void SimulateBullets(float time)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetBulletPosition(bullet);
            bullet.time += time;
            Vector3 p1 = GetBulletPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void DestroyBullets()
    {
        List<Bullet> nBullets = new List<Bullet>();
        bullets.ForEach(bullet =>
        {
            if (bullet.time >= maxLifeTime)
            {
                //Destroy(bullet.tracer);
            }
            else
            {
                nBullets.Add(bullet); 
            };
        });
        bullets.Clear();
        bullets = nBullets;
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitData, distance))
        {
            //Debug.Log($"Hit object {hitData.collider.gameObject.name}");
            //Debug.DrawLine(ray.origin, hitData.point, Color.red, 1.0f, false);
            var transform1 = bulletHitParticle.transform;
            transform1.position = hitData.point;
            transform1.forward = hitData.normal;
            bulletHitParticle.Play();
            bullet.tracer.transform.position = hitData.point;
            bullet.time = maxLifeTime;
            ApplyDamage(hitData.collider.gameObject, start);
            //Destroy(bullet.tracer);
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }


    public void StopFiring()
    {
        isFiring = false;
    }


    private void Fire()
    {
        muzzleFlashParticle.Play();
        playerStats.ReduceAmmo();

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        Bullet bullet = SpawmBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        weaponRecoil.GenerateRecoil();
        // ray.origin = raycastOrigin.position;
        // ray.direction = raycastOrigin.forward;
        // var tracer = Instantiate(bulletTracer, ray.origin, quaternion.identity);
        // tracer.AddPosition(ray.origin);
        // if (Physics.Raycast(ray, out hitData))
        // {
        //     //Debug.Log("Hit object");
        //     //Debug.DrawLine(ray.origin, hitData.point, Color.red, 1.0f, false);
        //     var transform1 = bulletHitParticle.transform;
        //     transform1.position = hitData.point;
        //     transform1.forward = hitData.normal;
        //     bulletHitParticle.Play();
        //
        //     tracer.transform.position = hitData.point;
        // }


    }

    
    void ApplyDamage(GameObject target, Vector3 pos){
        // if (target.CompareTag("AI"))
        // {
        //     target.GetComponent<EmeraldAI.EmeraldAISystem>().Damage(20, EmeraldAI.EmeraldAISystem.TargetType.Player, gameObject.transform, 400);
        // }
        switch (weaponMode)
        {
            case 0:
                SendDamage(target);
                break;
            case 1:
                Debug.Log($"Instantiate missile on the posiiton of {target.gameObject.name}");
                GameObject missle = Instantiate(missilePrefab, pos, quaternion.identity);
                break;
        }
    }

    void SendDamage(GameObject target)
    {
        if (target.CompareTag("AI"))
        {
            EmeraldAI.EmeraldAISystem AI = target.GetComponent<EmeraldAI.EmeraldAISystem>(); 
            AI.Damage(20, EmeraldAI.EmeraldAISystem.TargetType.Player, gameObject.transform, 400);
                 
        } 
    }

    public void SetBulletHitParticleByMode(int weaponModeCode)
    {
        switch (weaponModeCode)
        {
            case 0:
                bulletHitParticle = bulletHitParticlePrimary;
                break;
            case 1:
                bulletHitParticle = bulletHitParticleSecondary;
                break;
        }

        weaponMode = weaponModeCode;
    }

    public int GetWeaponMode()
    {
        return weaponMode;
    }
    

}
