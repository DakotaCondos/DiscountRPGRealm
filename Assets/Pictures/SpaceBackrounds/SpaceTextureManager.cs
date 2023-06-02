using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SpaceTextureManager : MonoBehaviour
{
    [SerializeField] private List<SpaceSO> emptySpaceList;
    [SerializeField] private List<Texture2D> emptyTextureList;

    [SerializeField] private List<SpaceSO> townSpaceList;
    [SerializeField] private List<Texture2D> townTextureList;

    [SerializeField] private List<SpaceSO> chanceSpaceList;
    [SerializeField] private List<Texture2D> chanceTextureList;

    [SerializeField] private List<SpaceSO> eventSpaceList;
    [SerializeField] private List<Texture2D> eventTextureList;

    [SerializeField] private List<SpaceSO> monsterSpawnSpaceList;
    [SerializeField] private List<Texture2D> monsterSpawnTextureList;

    [SerializeField] private List<SpaceSO> trapSpaceList;
    [SerializeField] private List<Texture2D> trapTextureList;

    [SerializeField] private List<SpaceSO> customSpaceList;
    [SerializeField] private List<Texture2D> customTextureList;

    [ContextMenu("Assign Textures To Spaces")]
    private void AssignTexturesToSpaces()
    {
        List<List<SpaceSO>> spaceLists = new List<List<SpaceSO>>
        {
            emptySpaceList,
            townSpaceList,
            chanceSpaceList,
            eventSpaceList,
            monsterSpawnSpaceList,
            trapSpaceList,
            customSpaceList
        };

        List<List<Texture2D>> textureLists = new List<List<Texture2D>>
        {
            emptyTextureList,
            townTextureList,
            chanceTextureList,
            eventTextureList,
            monsterSpawnTextureList,
            trapTextureList,
            customTextureList
        };

        for (int i = 0; i < spaceLists.Count; i++)
        {
            List<Texture2D> unassignedTextures = new List<Texture2D>(textureLists[i]);

            foreach (SpaceSO space in spaceLists[i])
            {
                space.spaceTextures.Clear();
                foreach (Texture2D texture in textureLists[i])
                {
                    string fileName = Path.GetFileNameWithoutExtension(texture.name).ToLower();
                    if (fileName.StartsWith(space.spaceName.ToLower()))
                    {
                        space.spaceTextures.Add(texture);
                        unassignedTextures.Remove(texture);

#if UNITY_EDITOR
                        EditorUtility.SetDirty(space);
#endif
                    }
                }
            }

            // Print out unassigned textures
            foreach (Texture2D texture in unassignedTextures)
            {
                Debug.Log("Texture was not assigned: " + texture.name);
            }


        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
}
