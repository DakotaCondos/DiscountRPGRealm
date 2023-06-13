using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Monster : IMoveable, IFightable
{
    public string MonsterName { get; private set; }
    public Texture2D monsterTexture;
    public int power;
    public bool doesMove;
    public AudioClip battlecry;
    public Space currentSpace = null;
    public Queue<PlayerEffect> effects = new();

    public Monster(string monsterName, Texture2D monsterTexture, int power, bool doesMove, AudioClip battlecry = null)
    {
        MonsterName = monsterName;
        this.monsterTexture = monsterTexture;
        this.power = power;
        this.doesMove = doesMove;
        this.battlecry = battlecry;
    }

    public Monster(MonsterSO so)
    {
        MonsterName = so.MonsterName;
        monsterTexture = so.monsterTextures[UnityEngine.Random.Range(0, so.monsterTextures.Count)];
        power = so.power;
        doesMove = so.doesMove;
        battlecry = so.battlecry;
    }

    public void Move(Space space, int speed)
    {
        throw new System.NotImplementedException();
    }
}