using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMine : MonoBehaviour, IInteractable, ICollectable, IUsable, INonAITarget
{
    
    public bool _IsActive;
    public Item _Item;
    
    public ParticleSystem avaliableParticle;
    public ParticleSystem activeParticle;

    private int health = 1;

    public Item Item => _Item;
    public bool IsActive 
    {
        get => this._IsActive;
        set => this._IsActive = value;
    }

    private void Awake()
    {
        UpdateState(); 
    }
    
    public void Use()
    {
        // GameObject obj = InventoryManager.Instance.SpawnItem(Item.prefab);
        // obj.GetComponent<IInteractable>().IsActive = true;
        // obj.gameObject.layer = LayerMask.NameToLayer("Interactables");
        IsActive = true;
        gameObject.layer = LayerMask.NameToLayer("Non_AI_Targets");
        gameObject.tag = "Non_AI_Targets";
        UpdateState();
    }

    void UpdateState()
    {
        if (IsActive)
        {
            avaliableParticle.Stop();
            activeParticle.Play();
            //GetComponentInChildren<ExplosionTrigger>().Activate(); 
        }
        else
        {
            activeParticle.Stop();
            avaliableParticle.Play();
        }
    }

    public void PendingDestroy()
    {
        Destroy(this.gameObject, 2f); 
    }
    
    public void OnInteract(GameObject obj)
    {
        Debug.Log($"OnInteract effect to non player: {gameObject.name}"); 
    }

    public void OnInteract()
    {
        Debug.Log("OnIntecrect Mine");
        if (IsActive) return;
        InventoryManager.Instance.AddItem(_Item);
        Destroy(gameObject);
    }

    public void OnDamaged(int amount)
    {
        if (health<= 0)
        {
            return;
        }
        
        Debug.Log($"Audio mine {gameObject.name} took {amount} damage"); 
        health -= amount;
        
        if(health <= 0){
            OnKilled();
        }
    }

    public void OnKilled()
    {
        Debug.Log("AudioMine destroyed, trigger explosion now");
        GameObject triggerObj = transform.Find("ExplosionTrigger").gameObject;
        ExplosionTrigger trigger = triggerObj.GetComponentInChildren<ExplosionTrigger>();
        triggerObj.SetActive(true);
        trigger.Activate(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsActive && collision.gameObject.CompareTag("Player"))
        {
            OnInteract(); 
        }
        else if(IsActive && collision.gameObject.CompareTag("AI"))
        {
            OnInteract(collision.gameObject);
        }
    }
    
}
