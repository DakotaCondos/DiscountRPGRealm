using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChanceCardCreator : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> textures = new();

    private Dictionary<ChanceCardType, List<string>> CardData = new Dictionary<ChanceCardType, List<string>>
    {
        {
            ChanceCardType.GainingMoney,
            new List<string> {
                "Leprechaun's Jackpot",
                "Treasure Chest Bonanza",
                "The Bank of Unlikely Fortune",
                "Slaying the Tax Beast",
                "Coupon Craziness",
                "Dungeon Delver's Discount",
                "Investing in Goblin Startups",
                "Magical Auction Mayhem",
                "Fairy Godmother's Favor",
                "The Golden Goose Gambit"
            }
        },
        {
            ChanceCardType.LosingMoney,
            new List<string> {
                "Orcish Toll Bridge",
                "Goblin Market Swindle",
                "Impish Pickpocket",
                "The Tax Collector's Revenge",
                "Magician's Vanishing Coin Trick",
                "Gambling Goblins",
                "Cursed Treasure Chest",
                "The Alchemist's Overpriced Elixir",
                "Dragon's Fine for Trespassing",
                "Misguided Investment Scheme"
            }
        },
        {
            ChanceCardType.GainingExperience,
            new List<string> {
                "Epic Battle Bragging Rights",
                "Quest for the Legendary Loaf of Bread",
                "The Bard's Tall Tales",
                "Monstrous Makeup Artist Workshop",
                "Mastering the Art of Potions",
                "Beast Taming Extravaganza",
                "Wizarding School Shenanigans",
                "Dungeon Cooking Competition",
                "The Trial of Ridiculous Riddles",
                "A Day in the Life of an NPC"
            }
        },
        {
            ChanceCardType.LosingExperience,
            new List<string> {
                "Goblin Prankster's Mischief",
                "Magician's Failed Experiment",
                "Trapped in the Mimic's Lair",
                "Epic Battle with a Ticklish Troll",
                "The Bard's Forgotten Lyrics",
                "Monster Snot Incident",
                "Potion Mixing Catastrophe",
                "The Disastrous Beast Taming Contest",
                "Wizarding School Pop Quiz",
                "The Trap Door of Humiliation"
            }
        },
        {
            ChanceCardType.GainingPower,
            new List<string> {
                "Magical Item Swap Meet",
                "Spellbook of Unexpected Spells",
                "Training with the Great Master",
                "Goblin Sidekick Recruitment Drive",
                "Potion Brewing Masterclass",
                "The Great Arm-Wrestling Tournament",
                "Wishing Well Shenanigans",
                "Pet Dragon Adoption Agency",
                "The Cursed Crown of Inconvenience",
                "Realm Conqueror's Discount Store"
            }
        },
        {
            ChanceCardType.LosingPower,
            new List<string> {
                "The Potion of Shrinking",
                "Cursed Amulet of Misfortune",
                "The Gauntlet of Tickles",
                "Magic-Draining Pixie Encounter",
                "The Ominous Robe of Clumsiness",
                "The Lost Wand of Bumbling",
                "The Unholy Banishment of Skills",
                "Epic Spell Backfire",
                "Petrification Curse Mishap",
                "The Chaos Mirror of Opposite Abilities"
            }
        }
    };

    [ContextMenu("Create Cards")]
    public void CreateCards()
    {
        foreach (var cardData in CardData)
        {
            foreach (var cardTitle in cardData.Value)
            {
                var newCard = ScriptableObject.CreateInstance<ChanceCardSO>();
                newCard.CardType = cardData.Key;
                newCard.cardTitle = cardTitle;
                foreach (var texture in textures)
                {
                    if (Sanitize(texture.name).StartsWith(Sanitize(cardTitle)))
                    {
                        newCard.images.Add(texture);
                    }
                }
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(newCard, $"Assets/ScriptableObjects/ChanceCards/{newCard.cardTitle}.asset");
                AssetDatabase.SaveAssets();
#endif
            }
        }
    }

    private string Sanitize(string input)
    {
        var output = new System.Text.StringBuilder();
        foreach (var ch in input.ToLower())
        {
            if (char.IsLetterOrDigit(ch))
            {
                output.Append(ch);
            }
        }
        return output.ToString();
    }
}
