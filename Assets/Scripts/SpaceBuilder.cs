using System.Collections;
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


    public void BuildSpace(Space space, SpaceType spaceType)
    {
        switch (spaceType)
        {
            case SpaceType.Empty:
                HandleEmptySpace(space);
                break;
            case SpaceType.Town:
                HandleTownSpace(space);
                break;
            case SpaceType.Chance:
                HandleChanceSpace(space);
                break;
            case SpaceType.Event:
                HandleEventSpace(space);
                break;
            case SpaceType.MonsterSpawn:
                HandleMonsterSpawnSpace(space);
                break;
            case SpaceType.Trap:
                HandleTrapSpace(space);
                break;
            default:
                break;
        }
    }

    public void HandleEmptySpace(Space space)
    {
        Debug.Log("Handling Empty Space");
        // Add code to handle the 'space' object here.
    }

    public void HandleTownSpace(Space space)
    {
        Debug.Log("Handling Town Space");
        // Add code to handle the 'space' object here.
    }

    public void HandleChanceSpace(Space space)
    {
        Debug.Log("Handling Chance Space");
        // Add code to handle the 'space' object here.
    }

    public void HandleEventSpace(Space space)
    {
        Debug.Log("Handling Event Space");
        // Add code to handle the 'space' object here.
    }

    public void HandleMonsterSpawnSpace(Space space)
    {
        Debug.Log("Handling Monster Spawn Space");
        // Add code to handle the 'space' object here.
    }

    public void HandleTrapSpace(Space space)
    {
        Debug.Log("Handling Trap Space");
        // Add code to handle the 'space' object here.
    }


}

