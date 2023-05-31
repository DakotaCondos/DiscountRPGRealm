using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpaceTextureManager : MonoBehaviour
{
    [SerializeField] private List<SpaceSO> spaceList;
    [SerializeField] private List<Texture2D> textureList;

    [ContextMenu("Assign Textures To Spaces")]
    private void AssignTexturesToSpaces()
    {
        List<Texture2D> unassignedTextures = new List<Texture2D>(textureList);

        foreach (SpaceSO space in spaceList)
        {
            space.spaceTextures.Clear(); // Clear old textures first, or it will keep adding textures when you re-run this.
            foreach (Texture2D texture in textureList)
            {
                string fileName = Path.GetFileNameWithoutExtension(texture.name).ToLower();
                if (fileName.StartsWith(space.spaceName.ToLower()))
                {
                    space.spaceTextures.Add(texture);
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
