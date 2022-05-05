using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionMenu;
    public AudioClip uiClickClip;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ToggleMainMenu()
    {
        mainMenu.SetActive(true);
        instructionMenu.SetActive(false);
        audioSource.PlayOneShot(uiClickClip);
    }
   
    public void ToggleInstructionsMenu()
    {
        mainMenu.SetActive(false);
        instructionMenu.SetActive(true);
        audioSource.PlayOneShot(uiClickClip);
    }
    
    public void StartGame()
    {
        Debug.Log("Start Game scene");
        SceneManager.LoadScene(1);
    }
    
    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
}
