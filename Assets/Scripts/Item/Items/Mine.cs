using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, ICollectable, IInteractable, IUsable
{
    
    public bool _IsActive;
    public Item _Item;
    
    public ParticleSystem avaliableParticle;
    public ParticleSystem activeParticle;
    
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
        gameObject.layer = LayerMask.NameToLayer("Interactables");
        //gameObject.tag = "Non_AI_Targets";
        UpdateState();
    }

    void UpdateState()
    {
        if (IsActive)
        {
            avaliableParticle.Stop();
            activeParticle.Play();
            GameObject triggerObj = transform.Find("ExplosionTrigger").gameObject;
            ExplosionTrigger trigger = triggerObj.GetComponentInChildren<ExplosionTrigger>();
            triggerObj.SetActive(true);
            trigger.Activate(); 
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
