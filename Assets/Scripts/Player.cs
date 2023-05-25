using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string PlayerName { get; private set; }
    public int TeamID { get; private set; }

    public Player(string playerName, int teamID = 0)
    {
        PlayerName = playerName;
        TeamID = teamID;
    }
}