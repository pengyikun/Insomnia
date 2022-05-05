using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationItemController : MonoBehaviour
{
    public Text textObj;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine("FadeOut");
        Destroy(this.gameObject, 5f); 
    }

    public void SetContent(string content)
    {
        textObj.text = content;
    }
    
    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(4f);
        image.CrossFadeAlpha(0.0f, .9f, false);
        textObj.CrossFadeAlpha(0.0f, .9f, false); 
    }
    
}
