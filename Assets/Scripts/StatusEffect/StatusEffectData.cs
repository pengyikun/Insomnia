using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public int id;
    public string effectName;
    public Sprite icon;
    public string description;
    public float dotAmount;
    public float tickSpeed;
    public float movementPenalty;
    public float statimaPenalty;
    public float lifeTime;

    public GameObject effectParticles;
}
