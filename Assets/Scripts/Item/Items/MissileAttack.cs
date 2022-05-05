using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MissileAttack : MonoBehaviour,IInteractable
{
    public bool IsActive { get; set; }
    private AudioSource audioSource;
    public AudioClip[] explosionSounds;
    private void Start()
    {
        IsActive = true;
        audioSource = GetComponent<AudioSource>();
        Invoke ("DisableCollider", .3f);
        PlaySound();
        Debug.Log("Destroy missile after .3 seconds");
        Destroy(this.gameObject, 2f);
    }

    

    public void OnInteract(GameObject obj)
    {
        Debug.Log("BOMB!!!");
        obj.GetComponent<EmeraldAI.EmeraldAISystem>().Damage(3000, EmeraldAI.EmeraldAISystem.TargetType.Player, gameObject.transform, 400);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"[CollisionEnter] {other.gameObject.tag} : {other.gameObject.name}");
        if (other.CompareTag("AI"))
        {
            OnInteract(other.gameObject);
        } 
    }
    void  DisableCollider () {
        Debug.Log("Disabled missile collider");
        GetComponent<SphereCollider>().enabled = false;
    }
    void PlaySound()
    {
        int n = Random.Range(1, explosionSounds.Length);
        audioSource.clip = explosionSounds[n];
        audioSource.PlayOneShot(audioSource.clip);
    }
    
    
    
    
}
