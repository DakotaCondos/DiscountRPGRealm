using UnityEngine;
using System.Collections.Generic;
using System.Text;

public enum GameDifficulty
{
    Relaxed = 65,
    Easy = 80,
    Normal = 100,
    Hard = 110,
    Insaine = 120,
}

public class GameManager : MonoBehaviour
{
    public List<Player> Players = new();
    public GameDifficulty gameDifficulty = GameDifficulty.Normal;

    public static GameManager Instance { get; private set; }

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

    public void AddPlayer(Player player)
    {
        Players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
    }

    public void ResetGame()
    {
        Players.Clear();
    }
}
