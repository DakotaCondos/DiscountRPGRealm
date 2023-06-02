using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBuilder : MonoBehaviour
{
    public List<SpaceSO> emptySpaces;
    public List<SpaceSO> townSpaces;
    public List<SpaceSO> chanceSpaces;
    public List<SpaceSO> eventSpaces;
    public List<SpaceSO> monsterSpawnSpaces;
    public List<SpaceSO> trapSpaces;


    public void BuildSpace(Space space)
    {
        switch (space.spaceType)
        {
            case SpaceType.Empty:
                BuildSpace(space, emptySpaces);
                break;
            case SpaceType.Town:
                BuildSpace(space, townSpaces);
                break;
            case SpaceType.Chance:
                BuildSpace(space, chanceSpaces);
                break;
            case SpaceType.Event:
                BuildSpace(space, eventSpaces);
                break;
            case SpaceType.MonsterSpawn:
                BuildSpace(space, monsterSpawnSpaces);
                break;
            case SpaceType.Trap:
                BuildSpace(space, trapSpaces);
                break;
            case SpaceType.Custom:
                BuildCustomSpace(space);
                break;
            default:
                break;
        }
    }

    private void BuildCustomSpace(Space space)
    {
        space.SetSpaceName(space.blueprint.spaceName);
        space.SetSpaceTexture(GetRandomItem(space.blueprint.spaceTextures));
        space.canMonstersTraverse = space.blueprint.canMonstersTraverse;
        space.canSpawnMonsters = space.blueprint.canSpawnMonsters;
        space.shopLevel = space.blueprint.shopLevel;
        space.spaceType = space.blueprint.customSpaceTypeOverride;
    }

    public void BuildSpace(Space space, List<SpaceSO> listSOs)
    {
        SpaceSO blueprint = GetRandomItem(listSOs, true);
        space.SetSpaceName(blueprint.spaceName);
        List<Texture2D> textures = blueprint.spaceTextures;
        space.SetSpaceTexture(GetRandomItem(textures));
        space.canMonstersTraverse = blueprint.canMonstersTraverse;
        space.canSpawnMonsters = blueprint.canSpawnMonsters;
        space.shopLevel = blueprint.shopLevel;
    }

    public T GetRandomItem<T>(List<T> items, bool removeFromListAfterSelecting = false)
    {
        if (items == null || items.Count == 0)
        {
            throw new ArgumentException("List cannot be null or empty.", nameof(items));
        }

        int randomIndex = UnityEngine.Random.Range(0, items.Count);
        T item = items[randomIndex];

        if (removeFromListAfterSelecting) { items.Remove(item); }

        return item;
    }

}

