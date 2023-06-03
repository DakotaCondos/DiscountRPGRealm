using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IMoveable, IFightable
{
    public string MonsterName { get; private set; }
    public Texture2D monsterTexture;
    public int power;
    public bool doesMove;
    public AudioClip battlecry;
    public Space currentSpace = null;

    public Monster(string monsterName, Texture2D monsterTexture, int power, bool doesMove, AudioClip battlecry = null)
    {
        MonsterName = monsterName;
        this.monsterTexture = monsterTexture;
        this.power = power;
        this.doesMove = doesMove;
        this.battlecry = battlecry;
    }

    public void Move(Space space, int speed)
    {
        throw new System.NotImplementedException();
    }
}
