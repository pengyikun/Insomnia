using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hitData;

    private void Start()
    {
       cam = Camera.main; 
    }

    private void Update()
    {
        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;
        Physics.Raycast(ray, out hitData);
        if (Physics.Raycast(ray, out hitData)) {
            transform.position = hitData.point;
        } else {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}
