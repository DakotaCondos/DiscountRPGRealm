using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Space", menuName = "ScriptableObjects/Space")]
public class SpaceSO : ScriptableObject
{
    public string spaceName;
    public List<Texture2D> spaceTextures;
    public bool canMonstersTraverse = true;
    public bool canSpawnMonsters = false;
    public int shopLevel = 0;
}
