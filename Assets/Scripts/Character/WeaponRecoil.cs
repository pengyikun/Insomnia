using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public PlayerWeaponLogic playerWeaponLogic;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource cameraShakeSource;
    public float verticalRecoil;
    public float duration;
    private float time;
    
    private void Awake()
    {
        playerWeaponLogic = gameObject.transform.parent.transform.parent.GetComponent<PlayerWeaponLogic>();
        cameraShakeSource = gameObject.GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if (time > 0)
        {
            playerWeaponLogic.cameraAxisY.Value -= (verticalRecoil * Time.deltaTime) /duration;
            time -= Time.deltaTime;
        }
       
    }


    public void GenerateRecoil()
    {
        time = duration;
        cameraShakeSource.GenerateImpulse(Camera.main.transform.forward);
    }
}
