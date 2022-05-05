using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItem", menuName ="Item/CreteNewItem")]

public class Item : ScriptableObject
{
    public int id;
    public bool spawnWhenUse;
    public string ItemName;
    public float weight;
    public Sprite icon;
    public GameObject prefab;
    public StatusEffectData[] statusEffects;
    public LayerMask effectObjectType;
}
