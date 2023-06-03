using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : IMoveable
{
    public string PlayerName { get; private set; }
    public int TeamID { get; private set; }
    public Color playerColor;
    public Texture2D playerTexture; //in this case just a pcture to attach to the UI block for the player piece

    public int xp = 0;
    [SerializeField] int xpRequiredToLevel = 3;
    public int level = 1;
    public int movementBonusPlayer = 0;
    public int movementBonusItems = 0;
    public int powerBonusPlayer = 0;
    public int powerBonusItems = 0;
    public Space currentSpace = null;
    public bool hasMoved = false;



    public Player(string playerName, int teamID, Color playerColor, Texture2D playerTexture = null)
    {
        PlayerName = playerName;
        TeamID = teamID;
        this.playerColor = playerColor;
        this.playerTexture = playerTexture;
    }

    public void AddXP(int value)
    {
        xp += value;
        int levelsGained = xp / xpRequiredToLevel;
        level += levelsGained;
        xp %= xpRequiredToLevel;
    }

    public int GetPower()
    {
        return level * 3 + powerBonusItems + powerBonusPlayer;
    }

    public int GetMovement()
    {
        int movementLevel = (level / 2 > 1) ? level / 2 : 1;
        return movementLevel + movementBonusItems + movementBonusPlayer;
    }

    public void Move(Space space, int speed)
    {
        throw new System.NotImplementedException();
    }
}
