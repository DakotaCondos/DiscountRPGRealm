using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.VersionControl.Asset;

public class SpaceSOCreator : MonoBehaviour
{
    private List<string> emptySpaceNames = new List<string>()
    {
        "Abandoned Farmstead",
        "Rundown Inn",
        "Old Wooden Bridge",
        "Riverside Fishing Village",
        "Tranquil Meadow",
        "Dense Thorn Forest",
        "Lone Guard Tower",
        "Ancient Standing Stones",
        "Bubbling Hot Springs",
        "Forgotten Cemetery",
        "Weathered Statue",
        "Broken Windmill",
        "Derelict Lighthouse",
        "Overgrown Orchard",
        "Crumbling Ruins",
        "Sleepy Hamlet",
        "Quiet Monastery",
        "Unmarked Crossroads",
        "Moss-Covered Cliffside",
        "Cracked Mountain Pass",
        "Shallow Reed Marsh",
        "Whispering Pine Woods",
        "Ramshackle Trading Post",
        "Withered Vineyard",
        "Eroded Sea Caves",
        "Secluded Beach Cove",
        "Sun-bleached Skeleton Field",
        "Dusty Desert Oasis",
        "Abandoned Mineshaft",
        "Gently Babbling Brook",
        "Hidden Waterfall Grotto",
        "Scenic Hilltop Lookout",
        "Charred Battlefield Remnants",
        "Sparse Open Plains",
        "Cold Mountain Lake",
        "Twisted Tree Grove",
        "Winding Valley Path",
        "Dimly Lit Tunnel",
        "Isolated Hermit's Hut",
        "Forgotten Old Well",
        "Distant Lumber Mill",
        "Windswept Dunes",
        "Vacant Stables",
        "Dilapidated Stone Archway",
        "Lonely Outpost",
        "Faintly Glowing Mushroom Field",
        "Suspended Rope Bridge",
        "Ghostly Shipwreck",
        "Fading Mural Wall",
        "Covered Wagon Campsite"
    };

    private List<string> townSpaceNames = new List<string>()
    {
        "Haven's Gate",
        "Ironholm",
        "Whispering Pines",
        "Shadowfen",
        "Emberfall",
        "Starhaven",
        "Silverbrook",
        "Frostholm",
        "Havenreach",
        "Stormwatch",
        "Oakheart Village"
    };
    private List<string> chanceSpaceNames = new List<string>()
    {
        "Mysterious Wanderer",
        "Meteor Shower",
        "Sudden Tavern Brawl",
        "Local Festival",
        "Bridge Out",
        "Magic Malfunction",
        "Fae Trickery",
        "Dragon's Awakening",
        "Lost Spirit",
        "Ancient Portal",
        "Goblin's Bargain",
        "Farm Invasion",
        "Baker's Bet",
        "Misplaced Book",
        "Moonlit Dance",
        "Dungeon Collapsing",
        "Random Pet"
    };
    private List<string> eventSpaceNames = new List<string>()
    {
        "Floating Isles",
        "Labyrinthine Dungeon",
        "Necropolis",
        "Crystaline Caverns",
        "Skyreach Spire",
        "Chronos Ruins",
        "Moonlit Grove",
        "The Abyssal Trench",
        "Solstice Citadel",
        "Celestial Observatory",
        "The Molten Forge",
        "Celestial Falls",
        "Vortex Pinnacle",
        "Subterranean Hive",
        "Twin Peaks"
    };
    private List<string> monsterSpawnSpaceNames = new List<string>()
    {
        "Dark Forest",
        "Abandoned Mines",
        "Sunken Ruins",
        "Cursed Graveyard",
        "Desolate Wastelands",
        "Ancient Pyramid",
        "Mysterious Cave",
        "Frozen Tundra",
        "Volcanic Lair",
        "Haunted Castle",
        "Labyrinthine Sewers",
        "Astral Plane",
        "Witch's Bog",
        "Ancient Battlefield",
        "Infernal Gateway"
    };
    private List<string> trapSpaceNames = new List<string>()
    {
        "Pitfall Pass",
        "Cavern of False Echoes",
        "Snarespire Citadel",
        "Deceiving Dunes",
        "Quicksand Quagmire",
        "Mirage Meadow",
        "Labyrinth of Lost Whispers",
        "Hollow Hills of Hypnosis",
        "Hall of Illusionary Walls",
        "Serpent's Snare Swamp",
        "Sandtrap Sanctuary",
        "Treacherous Treasure Trove",
        "Veil of Vanishing Vistas",
        "False Freedom Fjord",
        "Pernicious Promontory"
    };





    [ContextMenu("BuildSpaces")]
    private void BuildSpaces()
    {
        foreach (var spaceName in emptySpaceNames)
        {
            CreateSpace(spaceName);
        }
    }

    private void CreateSpace(string spaceName)
    {
        print("Creating" + spaceName);
        SpaceSO newSpace = ScriptableObject.CreateInstance<SpaceSO>();
        newSpace.spaceName = spaceName;
        newSpace.spaceTextures = new List<Texture2D>(); // Initialize with no textures
        newSpace.canMonstersTraverse = false;
        newSpace.canSpawnMonsters = false;

        // This line saves the ScriptableObject as an asset in your project's "Assets/ScriptableObjects/Spaces/Trap/" directory.
        // If the directory does not exist, you need to create it first.
        AssetDatabase.CreateAsset(newSpace, "Assets/ScriptableObjects/Spaces/Event/" + spaceName + ".asset");
        AssetDatabase.SaveAssets();

    }
}
