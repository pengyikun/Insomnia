using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
  public AudioClip[] audioClips;
  public AudioClip openInventoryClip;
  public AudioClip closeInventoryClip;
  public AudioClip changeMagClip;
  public AudioClip levelupClip;
  public AudioClip missionCompleteClip;
  public AudioClip newMissionClip;
  public AudioClip notificationClip;
  
  private AudioSource audioSource;

  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  void PlayFootStepSound()
  {
    AudioClip clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
    audioSource.PlayOneShot(clip);
  }

  public  void PlayOpenInventorySound()
  {
    audioSource.PlayOneShot(openInventoryClip); 
  }
  
  public  void PlayCloseInventorySound()
  {
    audioSource.PlayOneShot(closeInventoryClip); 
  }
  
  public  void PlayChangeMagSound()
  {
    audioSource.PlayOneShot(changeMagClip); 
  }
  
  public  void PlayGenericAnnouncerSound()
  {
    audioSource.PlayOneShot(levelupClip); 
  }
  
  public  void PlayNotificationSound()
  {
    audioSource.PlayOneShot(notificationClip); 
  }
  
}
