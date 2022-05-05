using UnityEngine;

public class StatusEffect
{
    public StatusEffectData effectData;
    public float currentEffectTime = 0f;
    public float nextTickTime = 0f;
    public GameObject effectParticles;

    public StatusEffect(StatusEffectData effectData, GameObject effectParticles)
    {
        this.effectData = effectData;
        this.effectParticles = effectParticles;
    }
}
