using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTransmitter : MonoBehaviour, INonAITarget
{
    public int health;
    public int maxHealth;
    private TextMesh healthIndicator;

    private void Start()
    {
        healthIndicator = transform.Find("HealthIndicator").GetComponent<TextMesh>();
        UpdateHealthIndicator();
    }

    public void OnDamaged(int amount)
    {
        Debug.Log("Non AI target got damaged");
        health -= amount;
        UpdateHealthIndicator();
        if (health <= 0)
        {
            OnKilled(); 
        }
    }

    public void OnKilled()
    {
        PlayerController controller = InventoryManager.Instance.GetPlayerController();
        controller.GetComponent<PlayerStats>().gameStates = "failed";
        controller.OnFailed();
        gameObject.tag = "Untagged";
        gameObject.layer = 0;
        Destroy(gameObject, 1f); 
    }


    public void UpdateHealthIndicator()
    {
        Debug.Log("Update " + $"{health} / {maxHealth}");
        healthIndicator.text = $"{health} / {maxHealth}";
    }
}
