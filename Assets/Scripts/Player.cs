using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string PlayerName { get; private set; }

    public Player(string playerName)
    {
        PlayerName = playerName;
    }
}
