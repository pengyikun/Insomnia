using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingScreen : MonoBehaviour
{
    Image overlay;
    private float time = 0f;
    private List<GameObject> controls = new List<GameObject>();
    private bool init; 

    private void Awake()
    {
        overlay = GetComponent<Image>();
        foreach (var componentsInChild in GetComponentsInChildren<Button>())
        {
            Debug.Log($"Added {componentsInChild.gameObject.name}");
            controls.Add(componentsInChild.gameObject);
            componentsInChild.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 2 && overlay.color.a > .6)
        {
            if (!init)
            {
                foreach (GameObject control in controls)
                {
                    control.SetActive(true);
                }

                init = true;
            }
            var temp = overlay.color;
            temp.a -= 0.001f;
            overlay.color = temp;
        } 
    }
}
