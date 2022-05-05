using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public float explosionTimeOut;
    public int damageAmount;
    public ParticleSystem explostionParticle;
    public AudioClip[] explosionSounds;
    private AudioSource audioSource;
    
    public bool isActivate = false;
    private bool triggered = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    public void Activate()
    {
        isActivate = true;
    }

    public void Trigger()
    {
        triggered = true;
        explostionParticle.Play();
        Invoke ("DisableCollider", explosionTimeOut);
        PlaySound();
        // Destroy(this.gameObject, 2f);
        Destroy(transform.parent.gameObject , 2f);
    }

    public void OnInteract(GameObject obj)
    {
        if (!triggered)
        {
            Trigger();
        }
        Debug.Log("BOMB!!!");
        obj.GetComponent<EmeraldAI.EmeraldAISystem>().Damage(damageAmount, EmeraldAI.EmeraldAISystem.TargetType.Player, gameObject.transform, 400);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"[CollisionEnter] {other.gameObject.tag} : {other.gameObject.name}");
        if (isActivate && other.CompareTag("AI"))
        {
            OnInteract(other.gameObject);
        } 
    }
    void  DisableCollider () {
        Debug.Log("Disabled ExplosionTrigger collider");
        GetComponent<CapsuleCollider>().enabled = false;
    }
    void PlaySound()
    {
        int n = Random.Range(1, explosionSounds.Length);
        audioSource.clip = explosionSounds[n];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
