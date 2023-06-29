using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpaceBuilder : MonoBehaviour
{
    public List<SpaceSO> emptySpaces;
    public List<SpaceSO> townSpaces;
    public List<SpaceSO> chanceSpaces;
    public List<SpaceSO> eventSpaces;
    public List<SpaceSO> monsterSpawnSpaces;
    public List<SpaceSO> trapSpaces;
    public List<SpaceSO> transportSpaces;
    public List<SpaceSO> shadowRealmSpaces;


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
            case SpaceType.Challenge:
                BuildSpace(space, trapSpaces);
                break;
            case SpaceType.Custom:
                BuildCustomSpace(space);
                break;
            case SpaceType.Transport:
                BuildSpace(space, transportSpaces, false);
                break;
            case SpaceType.Jail:
                BuildSpace(space, shadowRealmSpaces, false);
                break;
            case SpaceType.EndGame:
                break;
            default:
                break;
        }
    }

    private void BuildCustomSpace(Space space)
    {
        if (space.blueprints.Count == 0)
        {
            Debug.LogWarning($"Custom Space {name} does not have blueprint assigned");
            return;
        }
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { print($"Building space {space.name}"); }
        SpaceSO blueprint = space.blueprints[UnityEngine.Random.Range(0, space.blueprints.Count)];
        space.SetSpaceName(blueprint.spaceName);
        space.SetSpaceTexture(GetRandomItem(blueprint.spaceTextures, false));
        space.canMonstersTraverse = blueprint.canMonstersTraverse;
        space.canSpawnMonsters = blueprint.canSpawnMonsters;
        space.shopLevel = blueprint.shopLevel;
        space.spaceType = blueprint.customSpaceTypeOverride;
    }

    public void BuildSpace(Space space, List<SpaceSO> listSOs, bool removeFromListAfterSelecting = true)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { print($"Building space {space.name}"); }
        SpaceSO blueprint = GetRandomItem(listSOs, removeFromListAfterSelecting);
        space.SetSpaceName(blueprint.spaceName);
        List<Texture2D> textures = blueprint.spaceTextures;
        space.SetSpaceTexture(GetRandomItem(textures));
        space.canMonstersTraverse = blueprint.canMonstersTraverse;
        space.canSpawnMonsters = blueprint.canSpawnMonsters;
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

