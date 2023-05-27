using Nova;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersStartupPanel : MonoBehaviour
{
    public GameObject menuPrefab;
    public GameObject menuPrefabInstance;
    public GameObject playerRowPrefab;
    public Transform playerRowLocation;
    public TextBlock messageBlock;

    private List<string> funnyNames = new List<string>()
    {
        "Sir Laze-a-Lot",
        "Couch Potato",
        "Captain Procrastinator",
        "Lazy McLazyface",
        "Sleepyhead",
        "Great Slacker",
        "Dr. Doze-a-Lot",
        "Master of Leisure",
        "Lord of Laziness",
        "Sir Snoozalot",
        "Lazybones",
        "Noncommittal One",
        "Queen of Procrastination",
        "Sir Sluggish",
        "Captain Daydream",
        "His Majesty Rests-a-Lot",
        "Lazy Lou",
        "Sir Do-Nothing",
        "Baron Bedhead",
        "Princess Yawnalot",
        "Indolent One",
        "Chief of Inactivity",
        "King of Lethargy",
        "Relaxed Ruler",
        "Lady Sleepsalot",
        "Count Loaf",
        "Commander Naps-a-Lot",
        "Sluggard",
        "Queen of Downtime",
        "Baroness Dreamer",
        "Slothful One",
        "Duke of Laze",
        "Unmotivated Individual",
        "Lord Loafington",
        "Captain Dilly-Dally",
        "Sir Sleeps-a-Lot",
        "Lazy Larry",
        "Nonchalant One",
        "Empress of Slumber",
        "Sir Slouch-a-Lot",
        "Grandmaster Procrastinator",
        "Lady Layabout",
        "Indifferent Individual",
        "Duchess of Doze",
        "Listless Lark",
        "Baron Slumber",
        "Lazy Lucy",
        "Complacent One",
        "Sir Stay-in-Bed",
        "Master of Inertia",
        "Master of Inertia",
        "Duke of Daydreams",
        "Lady Lag-alot",
        "Lieutenant Leisure",
        "Sir Slumbers",
        "Prince of Pause",
        "Meme Majesty",
        "Dormant Dame",
        "Princess Procrastinator",
        "Viscount of Vacant",
        "Countess Couch",
        "Baroness Break",
        "Idle Idol",
        "RPG RestGiver",
        "Mage of Memes",
        "Friar of Fiction",
        "Bishop of Breaks",
        "Squire Snooze",
        "Jester of Jest",
        "Knight of Naps",
        "Marquess of Meme",
        "Countess of Comic Sans",
        "Duchess of Doodles",
        "Madame Minecraft",
        "Warlord of Whimsy",
        "Goddess of Game Pause",
        "Lord of Laughs",
        "Sultan of Slumber",
        "King of Keks",
        "Overlord of OOF",
        "Prince of Puns",
        "Queen of Quietus",
        "Rogue of Rest",
        "Ranger of Relaxation",
        "Chancellor of Chill",
        "Lady Lethargy",
        "Viceroy of Vapidity",
        "Wanderer of Web",
        "Paladin of Puns",
        "Archduke of AFK",
        "Duchess of Downtime",
        "Elf of Ease",
        "Fairy of Freetime",
        "Gnome of Naps",
        "Centurion of Chill",
        "Captain of Cat Memes",
        "Wizard of Whoopee",
        "Sagacious Snoozer",
        "King Keyboard Warrior",
        "Sorcerer of Slumbers",
        "Lady Layabout",
        "Indifferent Inquisitor",
        "Lackadaisical Laird",
        "Nonchalant Knight",
        "Tardy Thane",
        "Dreamy Duchess",
        "Leisurely Lady",
        "Lethargic Lord",
        "Sleepy Scribe",
        "Slumbering Smith",
        "Carefree Count",
        "Baron of Boredom",
        "Dormant Damsel",
        "Jest Jester",
        "Captain Comic-Relief",
        "Lady LOLZ",
        "RPG Rest-Master",
        "Princess of Procrastination",
        "Zzz-zsassin",
        "Chilling Chaplain",
        "Serenely Sellsword",
        "Easygoing Elementalist",
        "Wistful Warlock",
        "Meandering Mystic",
        "Basking Berserker",
        "Hibernating Herald",
        "Reposing Reaper",
        "Mopey Mercenary",
        "Tardy Templar",
        "Vacant Vagrant",
        "Quiescent Quartermaster",
        "Languid Lancer",
        "Placid Paladin",
        "Somnolent Summoner",
        "Slumbering Sorcerer",
        "Idle Illusionist",
        "Relaxing Ranger",
        "Dallying Duelist",
        "Musing Monk",
        "Sluggish Shaman",
        "Siesta Samurai",
        "Lazy Lich",
        "Tranquil Troubadour",
        "Procrastinating Priest",
        "Loafing Lycanthrope",
        "Serene Sellsword",
        "Wistful Warlock",
        "Chilling Chaplain",
        "Meandering Mystic",
        "Hibernating Herald",
        "Basking Berserker",
        "Idle Illusionist",
        "Relaxing Ranger",
        "Dallying Duelist",
        "Musing Monk",
        "Sluggish Shaman",
        "Siesta Samurai",
        "Lazy Lich",
        "Tranquil Troubadour",
        "Procrastinating Priest",
        "Loafing Lycanthrope"
    };
    private List<Texture2D> playerTokens = new();

    private void Start()
    {
        CreateMenu();
        foreach (Texture2D item in ApplicationManager.Instance.playerTokens)
        {
            playerTokens.Add(item);
        }
    }

    public void CreateMenu()
    {
        if (menuPrefabInstance != null) { return; }
        menuPrefabInstance = Instantiate(menuPrefab, gameObject.transform);
        menuPrefabInstance.GetComponent<AddPlayerPanel>().playersStartupPanel = this;
    }

    public void DestroyMenu()
    {
        Destroy(menuPrefabInstance);
    }

    public void CreatePlayer(string playerName, int team, Color color)
    {
        //create player in GameManager
        Player player = new(playerName, team, color, SelectAndRemove(playerTokens));
        GameManager.Instance.AddPlayer(player);

        //create ui for player
        GameObject item = Instantiate(playerRowPrefab, playerRowLocation);
        PlayerRow playerRow = item.GetComponent<PlayerRow>();
        playerRow.playerNameBlock.Text = playerName;
        playerRow.teamBlock.Text = team.ToString();
        playerRow.player = player;

        SetMessage("");
    }

    public string GetRandomFunnyName()
    {
        int randomIndex = UnityEngine.Random.Range(0, funnyNames.Count);
        string name = funnyNames[randomIndex];
        funnyNames.Remove(name);
        return name;
    }

    public void SetMessage(string message)
    {
        messageBlock.Text = message;
    }

    public void StartGame()
    {
        if (GameManager.Instance.Players.Count < 1)
        {
            SetMessage("At least one player is required to start the game");
            //possible vibrate effect here
            return;
        }

        SceneManager.LoadScene(2);
    }

    public T SelectAndRemove<T>(List<T> list)
    {
        if (list.Count == 0)
        {
            Debug.LogWarning("List is empty.");
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        T selectedItem = list[randomIndex];
        list.RemoveAt(randomIndex);

        return selectedItem;
    }
}
