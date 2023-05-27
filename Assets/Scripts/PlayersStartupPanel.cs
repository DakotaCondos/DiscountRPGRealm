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
        "The Great Slacker",
        "Dr. Doze-a-Lot",
        "Master of Leisure",
        "Lord of Laziness",
        "Sir Snoozalot",
        "Lazybones",
        "The Noncommittal One",
        "Queen of Procrastination",
        "Sir Sluggish",
        "Captain Daydream",
        "His Majesty Rests-a-Lot",
        "Lazy Lou",
        "Sir Do-Nothing",
        "Baron Bedhead",
        "Princess Yawnalot",
        "The Indolent One",
        "Chief of Inactivity",
        "King of Lethargy",
        "The Relaxed Ruler",
        "Lady Sleepsalot",
        "Count Loaf",
        "Commander Naps-a-Lot",
        "The Sluggard",
        "Queen of Downtime",
        "Baroness Dreamer",
        "The Slothful One",
        "Duke of Laze",
        "The Unmotivated Individual",
        "Lord Loafington",
        "Captain Dilly-Dally",
        "Sir Sleeps-a-Lot",
        "Lazy Larry",
        "The Nonchalant One",
        "Empress of Slumber",
        "Sir Slouch-a-Lot",
        "Grandmaster Procrastinator",
        "Lady Layabout",
        "The Indifferent Individual",
        "Duchess of Doze",
        "The Listless Lark",
        "Baron Slumber",
        "Lazy Lucy",
        "The Complacent One",
        "Sir Stay-in-Bed",
        "Master of Inertia"
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
