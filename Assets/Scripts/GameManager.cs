using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<Player> players = new();

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

    public void AddPlayer(string playerName)
    {
        players.Add(new Player(playerName));
    }

    public void RemovePlayer(Player player)
    {
        players.Remove(player);
    }
}
