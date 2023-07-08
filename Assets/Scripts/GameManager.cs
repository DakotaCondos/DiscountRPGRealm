using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class GameManager : MonoBehaviour
{
    public List<Player> Players = new();

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
