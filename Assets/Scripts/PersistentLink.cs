using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class PersistentLink : MonoBehaviour
{
    public ApplicationManager ApplicationManager;
    public GameManager GameManager;
    public AudioManager AudioManager;

    private void Awake()
    {
        ApplicationManager = ApplicationManager.Instance;
        GameManager = GameManager.Instance;
        AudioManager = AudioManager.Instance;
    }

    private void Start()
    {
        if (ApplicationManager == null) Debug.LogWarning("PersistentLink could not find ApplicationManager");
        if (GameManager == null) Debug.LogWarning("PersistentLink could not find GameManager");
        if (AudioManager == null) Debug.LogWarning("PersistentLink could not find AudioManager");
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.SetSFXVolume(volume);
    }

    public void SetUIEffectsVolume(float volume)
    {
        AudioManager.SetUIEffectsVolume(volume);
    }


    public void Quit()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
