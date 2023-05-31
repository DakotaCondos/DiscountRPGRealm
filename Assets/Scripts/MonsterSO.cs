using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObjects/Monster")]
public class MonsterSO : ScriptableObject
{
    public string MonsterName;
    public int power;
    public bool doesMove;
    public AudioClip battlecry;
    public List<Texture2D> monsterTextures;
}
