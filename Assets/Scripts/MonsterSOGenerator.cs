using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterSOGenerator : MonoBehaviour
{
    [ContextMenu("BuildFromList")]
    public void CreateMonsterSOs()
    {
        var monsters = new Dictionary<string, int>
        {
            {"Hamster", 1},
            {"Goblin", 5},
            {"Orc", 10},
            {"Skeleton Archer", 12},
            {"Giant Spider", 15},
            {"Harpy", 18},
            {"Centaur", 20},
            {"Siren", 22},
            {"Minotaur", 25},
            {"Werewolf", 30},
            {"Troll", 35},
            {"Manticore", 40},
            {"Hydra", 45},
            {"Vampire", 50},
            {"Wraith", 55},
            {"Cyclops", 60},
            {"Basilisk", 65},
            {"Chimera", 70},
            {"Dragon", 75},
            {"Lich", 80},
            {"Deity", 100},
            {"Griffin", 45},
            {"Gargoyle", 40},
            {"Banshee", 38},
            {"Doppelganger", 35},
            {"Djinn", 50},
            {"Poltergeist", 25},
            {"Kraken", 80},
            {"Phoenix", 65},
            {"Yeti", 35},
            {"Merfolk", 20},
            {"Ghost", 25},
            {"Zombie", 10},
            {"Succubus", 60},
            {"Gorgon", 70},
            {"Demigod", 90}
        };

        foreach (var monster in monsters)
        {
            var monsterSO = ScriptableObject.CreateInstance<MonsterSO>();
            monsterSO.MonsterName = monster.Key;
            monsterSO.power = monster.Value;
            monsterSO.doesMove = true;

            // Remember to assign the battlecry and monsterTexture

            AssetDatabase.CreateAsset(monsterSO, $"Assets/ScriptableObjects/Monsters/{monster.Key}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
