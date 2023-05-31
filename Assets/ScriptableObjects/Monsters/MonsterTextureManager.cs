using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class MonsterTextureManager : MonoBehaviour
{
    [SerializeField] private List<MonsterSO> monsterList;
    [SerializeField] private List<Texture2D> textureList;

    [ContextMenu("Assign Textures To Monsters")]
    private void AssignTexturesToMonsters()
    {
        List<Texture2D> unassignedTextures = new List<Texture2D>(textureList);

        foreach (MonsterSO monster in monsterList)
        {
            monster.monsterTextures.Clear(); // Clear old textures first, or it will keep adding textures when you re-run this.
            foreach (Texture2D texture in textureList)
            {
                string fileName = Path.GetFileNameWithoutExtension(texture.name).ToLower();
                if (fileName.StartsWith(monster.MonsterName.ToLower()))
                {
                    monster.monsterTextures.Add(texture);
                    unassignedTextures.Remove(texture); // Remove from unassigned textures
                }
            }
        }

        // Print out unassigned textures
        foreach (Texture2D texture in unassignedTextures)
        {
            Debug.Log("Texture was not assigned: " + texture.name);
        }
    }
}
