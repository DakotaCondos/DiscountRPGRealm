using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnActor
{
    public bool isPlayer;
    public Player player;

    public TurnActor(Player player, bool isPlayer = true)
    {
        this.isPlayer = isPlayer;
        this.player = player;
    }
}
