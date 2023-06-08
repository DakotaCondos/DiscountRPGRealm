using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    // Making the ApplicationManager a Singleton
    public static ApplicationManager Instance { get; private set; }

    public List<Texture2D> playerTokens = new();
    public bool handlerNotificationsEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InitializeGame();

        // Load Main Menu
        SceneManager.LoadScene(1);
    }

    private void InitializeGame()
    {
        // Perform initialization tasks as required
    }

}
