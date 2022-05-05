using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectStores : MonoBehaviour
{
    public static StatusEffectStores Instance;

    public StatusEffectData overWeight;
    public StatusEffectData lowStamina;
    public StatusEffectData changeMag;

    private void Awake()
    {
        Instance = this;
    }
}
