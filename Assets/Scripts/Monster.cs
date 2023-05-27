using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string MonsterName { get; private set; }
    public Texture2D monsterTexture;
    public int power;
    public bool doesMove;
    public AudioClip battlecry;

    public Monster(string monsterName, Texture2D monsterTexture, int power, bool doesMove, AudioClip battlecry = null)
    {
        MonsterName = monsterName;
        this.monsterTexture = monsterTexture;
        this.power = power;
        this.doesMove = doesMove;
        this.battlecry = battlecry;
    }
}
