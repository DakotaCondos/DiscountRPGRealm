using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBuilder : MonoBehaviour
{
    public List<SpaceSO> emptySpaces = new();
    public List<SpaceSO> townSpaces = new();
    public List<SpaceSO> chanceSpaces = new();
    public List<SpaceSO> eventSpaces = new();
    public List<SpaceSO> monsterSpawnSpaces = new();
    public List<SpaceSO> trapSpaces = new();


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
            default:
                // Types not listed above are not randomly generated
                break;
        }
    }

    public void BuildSpace(Space space, List<SpaceSO> listSOs)
    {
        SpaceSO blueprint = GetRandomItem(listSOs, true);
        space.SetSpaceName(blueprint.spaceName);
        space.SetSpaceTexture(GetRandomItem(blueprint.spaceTextures));
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

