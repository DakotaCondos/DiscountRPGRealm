using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentLink : MonoBehaviour
{
    public ApplicationManager ApplicationManager;
    public GameManager GameManager;
    public AudioManager AudioManager;

    private void Awake()
    {
        ApplicationManager = FindAnyObjectByType<ApplicationManager>();
        GameManager = FindAnyObjectByType<GameManager>();
        AudioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Start()
    {
        if (ApplicationManager == null) Debug.LogWarning("PersistentLink could not find ApplicationManager");
        if (GameManager == null) Debug.LogWarning("PersistentLink could not find GameManager");
        if (AudioManager == null) Debug.LogWarning("PersistentLink could not find AudioManager");
    }
}
