using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;


public class PlayerWeaponLogic : MonoBehaviour
{
    public Transform cameraLookAt;
    public Cinemachine.AxisState cameraAxisX;
    public Cinemachine.AxisState cameraAxisY;

    public float turnSpeed = 15f;
    public float aimDuration = .3f;
    
    private Camera cam;
    private WeaponRaycastHandler weaponRaycastHandler;

    private Animator animator;
    private int isAiming = Animator.StringToHash("isAiming");
    
    public Rig aimLayer;

    public float gunFireSoundInterval = .1f;
    private AudioClip[] gunFireSounds;
    public AudioClip[] gunFireSoundsPrimary;
    public AudioClip[] gunFireSoundsSecondary;
    private AudioSource audioSource;
    private float gunFireSoundCounter = 1;

    private UIManager uiManager;
    private PlayerStats playerStats;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        weaponRaycastHandler = GetComponentInChildren<WeaponRaycastHandler>();
        audioSource = GetComponent<AudioSource>();
        uiManager = UIManager.Instance;
        playerStats = GetComponent<PlayerStats>();
        gunFireSounds = gunFireSoundsPrimary;
    }

    private void FixedUpdate()
    {
        cameraAxisX.Update(Time.fixedDeltaTime);
        cameraAxisY.Update(Time.fixedDeltaTime);

        if (cameraAxisX.Value != 0 || cameraAxisY.Value != 0)
        {
            cameraLookAt.eulerAngles = new Vector3(cameraAxisY.Value, cameraAxisX.Value, 0); 
        }
        
        
        float yawCamera = cam.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime );
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aimLayer.weight += Time.deltaTime / aimDuration;
            animator.SetBool(isAiming, true);
            uiManager.ToggleCrossHair(true);
        }
        else
        {
            aimLayer.weight -= Time.deltaTime / aimDuration; 
            animator.SetBool(isAiming, false);
            uiManager.ToggleCrossHair(false);
        }

        if (Input.GetButtonDown("Fire1") && playerStats.CanFire())
        {
                weaponRaycastHandler.StartFiring();
        }

        if (weaponRaycastHandler.isFiring && playerStats.CanFire())
        {
            PlayShootingSound();
            weaponRaycastHandler.UpdateFiringSequence(Time.deltaTime);
        }
        weaponRaycastHandler.UpdateBullets(Time.deltaTime);
        
        if (Input.GetButtonUp("Fire1") || !playerStats.canFire)
        {
            weaponRaycastHandler.StopFiring();
        }
    }
    
    void PlayShootingSound()
    {
        //Debug.Log(gunFireSoundCounter);
        if (gunFireSoundCounter < gunFireSoundInterval)
        {
            gunFireSoundCounter += Time.deltaTime; 
            return;
        }
        gunFireSoundCounter = 0.0f;
        //Debug.Log("Playing now");
        int n = Random.Range(1, gunFireSounds.Length);
        audioSource.clip = gunFireSounds[n];
        //audioSource.volume = volume;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void AdjustWeaponConfig()
    {
        int weaponMpde = playerStats.GetWeaponMode();
        switch (weaponMpde)
        {
            case 0:
                weaponRaycastHandler.fireRate = 8;
                weaponRaycastHandler.bulletSpeed = 1000;
                weaponRaycastHandler.bulletDrop = 0;
                gunFireSounds = gunFireSoundsPrimary;
                gunFireSoundInterval = .1f;
                break;
            case 1:
                weaponRaycastHandler.fireRate = 1;
                weaponRaycastHandler.bulletSpeed = 70;
                weaponRaycastHandler.bulletDrop = 50;
                gunFireSounds = gunFireSoundsSecondary;
                gunFireSoundInterval = 1f;
                break; 
        }
        weaponRaycastHandler.SetBulletHitParticleByMode(weaponMpde);

    }
}
