using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IStatusEffectable
{
    public StatusEffectData debugStatusEffectData;
    [SerializeField] private Dictionary<int, StatusEffect> statusEffects = new Dictionary<int, StatusEffect>();
    private List<StatusEffect> statusEffectsList = new List<StatusEffect>();
    
    public float health =100;
    public float maxHealth = 100;
    public float stamina = 100;
    public float maxStamina = 100;
    public float staminaConsumeRate = .05f;
    public float staminaRecoverRate = .1f;
    public float attackConsumeStaminaRate = 3f;
    public float jumpConsumeStaminaRate = 5f;
    public float weight = 0;
    public float maxWeight = 100;
    public int level = 1;
    public float exp = 0;
    public float levelUpExp = 300;
    public int gold = 0;
    public float maxSpeed = 5f;
    public float jumpForce = 10f;
    
    public bool canWalk = true;
    public bool canJump = true;
    public bool canFire = true;

    private float speedStatusEffectRate = 1.0f;
    private int weaponMode = 0;

    private int ammo = 30;
    public int maxAmmo;

    public string gameStates = "ongoing";
    
    

    public void AddHealth(float nHealth)
    {
        health = health + nHealth;
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public bool ReduceHealth(int nHealth)
    {
        health = health - nHealth;
        // TODO: Add Effects Check: Player could have Status Effects (Buff like Invincible)
        return health > 0;
    }

    public void AddStamina(float nStamina)
    {
        stamina = stamina + nStamina;
        if (stamina >= maxStamina)
        {
            stamina = maxStamina;
        }
        canJump = stamina > jumpConsumeStaminaRate;
    }

    public void ReduceStamina(float nStamina)
    {
        stamina = stamina - nStamina;
        if(stamina <= 0)
        {
            stamina = 0;
        }
        canJump = stamina > jumpConsumeStaminaRate;
    }

    public void AddGold(int nGold)
    {
        gold = gold + nGold;
    }

    public bool ReduceGold(int nGold)
    {
        if(gold < nGold)
        {
            return false;
        }
        gold = gold - nGold;
        return true;
    }


    public void SetWeight(float nWeight)
    {
        weight = nWeight;
        // TODO: Add/Remove Overweight status
        if (IsOverWeight())
        {
            ApplyEffect(StatusEffectStores.Instance.overWeight);
        }
        else if(statusEffects.ContainsKey(StatusEffectStores.Instance.overWeight.id))
        {
            RemoveEffect(statusEffects[StatusEffectStores.Instance.overWeight.id]);
        }
    }

    public void AddExp(float nExp)
    {
        exp = exp + nExp;
        if (!(exp >= levelUpExp)) return;
        LevelUp();
    }

    public int GetAmmo()
    {
        return ammo;
    }
    
    public void ReduceAmmo()
    {
        int amount = weaponMode == 0 ? 1 : 5;
        if (!CanFire())
        {
            return;
        }

        ammo -= amount;

        if (ammo <= 0)
        {
           ReloadMag(); 
        }

    }

    public bool CanFire()
    {
        int amount = weaponMode == 0 ? 1 : 5;
        if (amount > ammo)
        {
            return false;
        }

        return canFire;
    }

    public void ReloadMag()
    {
        ammo = 0;
        GetComponent<CharacterSoundController>().PlayChangeMagSound();
        ApplyEffect(StatusEffectStores.Instance.changeMag); 
    }

    public void LevelUp()
    {
        level += 1;
        jumpForce += .1f;
        maxSpeed += .1f;
        maxWeight += 10;
        maxHealth += 5;
        maxStamina += 3;
        levelUpExp = 300 + level*400;
        AddHealth(50);
        AddStamina(30);
        AddGold(30);
        UIManager.Instance.UpdatePlayerInfoUI();
        GetComponent<PlayerController>().PlayParticle("levelUp"); 
        Debug.Log("Levelled up to " + level);
        UIManager.Instance.PushNotification($"Leveled up to {level}"); 
        GetComponent<CharacterSoundController>().PlayGenericAnnouncerSound();
        // Incase Added exp larger than current levelup exp
        AddExp(0);
    }

    public List<StatusEffect> GetStatusEffectsList()
    {
        return statusEffectsList;
    }

    public void IterateEffects()
    {
        if (IsLowStamina())
        {
            if(!statusEffects.ContainsKey(4)) ApplyEffect(StatusEffectStores.Instance.lowStamina);
        }
        else
        {
            if(statusEffects.ContainsKey(4)) RemoveEffect(statusEffects[4]); 
        }
        
        statusEffectsList = statusEffects.Values.ToList();
        foreach (StatusEffect statusEffect in statusEffectsList)
        {
            HandleEffect(statusEffect);
        } 
    }
    
    public void ApplyEffect(StatusEffectData effectData)
    {
        //Debug.Log($"Apply effect {effectData.effectName}");

        if (!statusEffects.ContainsKey(effectData.id))
        {
            Debug.Log($"Added new effect {effectData.name}");
            GameObject effectParticles = effectData.effectParticles != null?Instantiate(effectData.effectParticles, transform): null;
            statusEffects.Add(effectData.id, new StatusEffect(effectData, effectParticles));
        }
        else
        {
            // Reset timer for duplicated existing effect
            Debug.Log($"Reset effect timer for {effectData.name}");
            StatusEffect statusEffect = statusEffects[effectData.id];
            statusEffect.currentEffectTime = 0;
            statusEffect.nextTickTime = 0;
        }
        
    }

    public void RemoveEffect(StatusEffect statusEffect)
    {
        //Debug.Log($"Remove effect {statusEffect.effectData.effectName}");
        if (statusEffect.effectData.movementPenalty != 0)
        {
            speedStatusEffectRate = 1f;
        }

        if (statusEffects.ContainsKey(statusEffect.effectData.id))
        {
            Debug.Log($"Removing effect {statusEffect.effectData.effectName} : id{statusEffect.effectData.id}");
            if(statusEffect.effectParticles != null) Destroy(statusEffect.effectParticles);
            statusEffects.Remove(statusEffect.effectData.id);
            
            //For ChangeMag
            if (statusEffect.effectData.id == 6)
            {
                //canFire = true;
                ammo = maxAmmo;
            }
        } 
    }

    public void HandleEffect(StatusEffect statusEffect)
    {
       //Debug.Log($"Handle effect {statusEffect.effectData.effectName}, {statusEffect.currentEffectTime}");
       
       statusEffect.currentEffectTime += Time.deltaTime;
       if (statusEffect.currentEffectTime >= statusEffect.effectData.lifeTime)
       {
           RemoveEffect(statusEffect);
           return;
       }

       if (statusEffect.currentEffectTime <= statusEffect.nextTickTime)
       {
           return;
       }

       if (statusEffect.effectData.dotAmount != 0)
       {
           statusEffect.nextTickTime += statusEffect.effectData.tickSpeed;
           //health -= statusEffect.effectData.dotAmount;
           AddHealth(statusEffect.effectData.dotAmount * -1f);
       }
       if (statusEffect.effectData.movementPenalty != 0)
       {
           statusEffect.nextTickTime += statusEffect.effectData.tickSpeed;
           speedStatusEffectRate = statusEffect.effectData.movementPenalty;
       }

       // For ChangeMag
       if (statusEffect.effectData.id == 6)
       {
           //canFire = false;
       }
    }
    

    public float CalculateSpeedOffset()
    {
        float offset = 1f;

        if (stamina <= 3)
        {
            offset *= .3f;
        }else if (stamina <= 8)
        {
            offset *= .8f;
        }

        if (weight <= maxWeight * 0.9)
        {
            offset *= 1.0f;
        }
        else if (weight < maxWeight)
        {
            offset *= 0.8f;
        }
        else if (weight < maxWeight + 50)
        {
            offset *= .4f;
        }
        else
        {
            offset *= .3f;
        }

        // Apply StatusEffect for speed
        offset *= speedStatusEffectRate;

        return offset >= .3f ? offset : .3f;
    }

    public float CalculateStaminaConsumeOffset()
    {
        if (weight <= maxWeight * 0.7)
        {
            return 1.0f;
        }
        else if (weight < maxWeight)
        {
            return 1.5f;
        }
        else
        {
            return 2f;
        }
    }

    public float CalculateJumpForceOffset()
    {

        if (weight <= maxWeight * 0.8)
        {
            return 1.0f;
        }
        else if (weight < maxWeight)
        {
            return 0.95f;
        }
        else if (weight < maxWeight + 50)
        {
            return .9f;
        }
        else
        {
            return .8f;
        }

    }

    public bool IsOverWeight()
    {
        return weight >= maxWeight * 0.8;
    }
    
    public bool IsLowStamina()
    {
        return stamina <= 3;
    }

    public int GetWeaponMode()
    {
        return weaponMode;
    }

    public void SetWeaponMode(int modeId)
    {
        weaponMode = modeId;
    }

}
