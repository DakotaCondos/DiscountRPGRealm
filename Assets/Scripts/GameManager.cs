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

    [ContextMenu("Print Players")]
    public void PrintPlayers()
    {
        if (Players.Count == 0) { print("No Players in list"); }

        StringBuilder sb = new StringBuilder();
        foreach (var item in Players)
        {
            sb.AppendLine($"{item.PlayerName} : Team {item.TeamID}");
        }

        print(sb);
    }
}
