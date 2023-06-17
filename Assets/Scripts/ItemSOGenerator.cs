using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class ItemSOGenerator : MonoBehaviour
{
    [SerializeField] private List<Texture2D> textureList;


    [ContextMenu("CreateEquipableItemSOs")]
    public void CreateEquipableItemSOs()
    {
        // {basePower, PvmBonusPower, PvpBonusPower , Movement, Money, "name", "Description"}
        var rawItemSOs = new List<(int, int, int, int, int, string, string)>()
        {
            // power
            (1, 0, 0, 0, 1, "Stick", "The ultimate weapon for when you want to poke things without commitment."),
            (3, 0, 0, 0, 5, "Rusty Sword", "It's like a regular sword, but with extra character (and rust)."),
            (8, 0, 0, 0, 15, "Sword", "The classic choice for those who want to stab their problems away."),
            (16,0, 0, -1, 35, "Big Sword", "Who needs finesse when you can swing this beast around like a maniac?"),
            (32,0, 0, 0, 80, "Fancy Sword", "For the discerning adventurer who wants to look sharp while slaying monsters."),
            (55,0, 0, 0, 120, "Legendary Sword", "A sword so legendary, it comes with its own fan club."),

            // pvp
            (2, -1, 1, 0, 5, "Rusty Dagger", "Perfect for backstabbing and spreading tetanus simultaneously."),
            (6, -2, 3, 0, 15, "Dagger", "A handy tool for those who prefer quick and precise slicing."),
            (14, -4, 6, 0, 35, "Poison Dagger", "The dagger that keeps on giving, lethal and sneaky."),
            (28, -7, 10, 0, 80, "Fancy Dagger", "When regular daggers just won't cut it, go fancy or go home."),
            (48, -11, 16, 0, 120, "Legendary Dagger", "A dagger of such renown that it has its own theme song."),

            // pvm
            (2, 1, -1, 0, 5, "Rusty Axe", "Guaranteed to make any lumberjack cry tears of disappointment."),
            (6, 3, -2, 0, 15, "Axe", "Swing this trusty companion and turn foes into firewood with a satisfying thwack!"),
            (14,6 ,-4 , -1, 35, "Big Axe", "Because sometimes you need to make a statement by cleaving things in half."),
            (28,10 ,-7 , 0, 80, "Fancy Axe", "A stylish accessory for decimating enemies and making firewood."),
            (48,16 ,-11 , 0, 120, "Legendary Axe", "With this axe, you'll be known as the ultimate tree slayer."),

            // Movement
            (0, -3, -3, 1, 10, "Single Boot", "The unfortunate sole survivor of its pair, great for limping around in style."),
            (0, 0, -3, 1, 20, "Boots", "For adventurers who want to kick butt and look fabulous at the same time."),
            (0, 0, 0, 2, 30, "Running Shoes", "Strap these on and become the Usain Bolt of the fantasy world."),
            (10, 0, 0, 2, 45, "Bicycle", "The steed of choice for adventurers who enjoy a leisurely ride into battle."),
            (15, 0, 0, 3, 60, "Tricycle", "A three-wheeled wonder that combines style, stability, and questionable combat effectiveness."),

            // Special
            (25, 5, 5, 1, 130, "Magic Wand", "The wand that makes dreams come true, or at least turns rabbits into top hats."),
            (50, 10, 0, 1, 325, "Magic Staff", "A weathered staff, once carried by a wise old soul who loves fireworks and grey robes."),
            (75, 0, 0, 0, 325, "Revolver", "A gun so powerful it can shoot holes in both reality and the fourth wall."),
            (65, 25, 0, 0, 325, "Dragon Slayer", "A sword so large, dropping it on enemies is enough to defeat them."),
            (65, 0, 25, 0, 325, "Hidden Blade", "The perfect accessory for any assassin, with a minor risk of finger amputation."),
        };


        foreach (var so in rawItemSOs)
        {
            var itemSO = ScriptableObject.CreateInstance<ItemSO>();
            if (so.Item1 != 0) { itemSO.itemEffects.Add(new(ItemEffectType.Power, so.Item1)); }
            if (so.Item2 != 0) { itemSO.itemEffects.Add(new(ItemEffectType.PowerVsMonster, so.Item2)); }
            if (so.Item3 != 0) { itemSO.itemEffects.Add(new(ItemEffectType.PowerVsPlayer, so.Item3)); }
            if (so.Item4 != 0) { itemSO.itemEffects.Add(new(ItemEffectType.Movement, so.Item4)); }
            itemSO.itemValue = so.Item5;
            itemSO.itemName = so.Item6;
            itemSO.itemDescription = so.Item7;
            itemSO.isConsumable = false;

            foreach (Texture2D texture in textureList)
            {
                string fileName = Path.GetFileNameWithoutExtension(texture.name).ToLower();
                if (fileName.StartsWith(itemSO.itemName.ToLower()))
                {
                    itemSO.image = texture;
                }
            }

            if (itemSO.image == null) { Debug.LogWarning($"{itemSO.itemName} image not assigned"); }

            AssetDatabase.CreateAsset(itemSO, $"Assets/ScriptableObjects/Items/{itemSO.itemName}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
