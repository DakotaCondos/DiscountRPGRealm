using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpaceType
{
    Empty,
    Town,
    Chance,
    Event,
    MonsterSpawn,
    Challenge,
    Custom,
    Transport,
    Jail,
    EndGame,
}

public class Space : MonoBehaviour
{
    // Setup Variable
    [Header("Setup")]
    public SpaceType spaceType;
    public bool canMonstersTraverse = true;
    public bool canSpawnMonsters = true;
    public int shopLevel = 0;
    public bool hasMandatoryEvent = false;
    public bool isStartingSpace = false;
    public List<Space> ConnectedSpaces = new List<Space>();
    public List<SpaceSO> blueprints = new();
    public Space TeleportToSpace = null;
    public bool customConnections = false;
    public Texture2D image;

    // Setup Static
    [Header("Static")]
    public GameObject lineDrawPoint;
    public GameObject monsterPiecePrefab;
    public Transform monsterPieceLocation;
    public Transform pieceMovePoint;
    public Transform spawnStartPoint;
    public UIBlock2D spaceBackingPanel;
    public UIBlock2D spaceGraphicPanel;
    public GameObject playerPiecePrefab;
    public Transform playerPieceLocation;
    public TextBlock namePlate;
    public Interactable interactableNova;
    [SerializeField] private GameObject _connectionRadius;
    [SerializeField] private UIBlock2D _monsterPieceUIBlock;
    [SerializeField] private UIBlock2D _playerPieceUIBlock;
    [SerializeField] private List<MonsterSO> _staticMonsterSOs = new();

    // Current State
    [Header("Current State")]
    public Monster monsterAtSpace;
    public bool hasMonster = false;
    public GameObject monsterPieceAtSpace;
    public List<Player> playersAtSpace;
    public List<GameObject> playersPieces = new();
    public bool IsBlocking = false;
    private TurnManager turnManager;

    // Other
    private Coroutine colorCoroutine; // Reference to the coroutine

    private void Awake()
    {
        playersAtSpace = new();
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void Initialize()
    {
        // Set Players at starting space
        if (isStartingSpace)
        {
            foreach (TurnActor turnActor in turnManager.GetUpcomingPlayers())
            {
                if (turnActor.isPlayer)
                {
                    AddPlayerToSpace(turnActor.player);
                }
            }
            AddPlayerToSpace(turnManager.GetCurrentActor().player);
        }

        if (_staticMonsterSOs.Count > 0)
        {
            Monster monster = new(_staticMonsterSOs[Random.Range(0, _staticMonsterSOs.Count)]);

            //difficulty modifier
            int difficultyModifier = (int)GameManager.Instance.gameDifficulty;
            monster.power = Mathf.RoundToInt(monster.power * (difficultyModifier / 100f));

            MonsterManager.Instance.monsters.Add(monster);
            AddMonsterToSpace(monster);
        }

        // Turn off dev UI elements
        _connectionRadius.SetActive(false);
        _playerPieceUIBlock.Border.Enabled = false;
        _monsterPieceUIBlock.Border.Enabled = false;
    }

    public void SelectSpace()
    {
        TurnState.TriggerEndMovement(turnManager.GetCurrentActor(), this);
    }

    public void TriggerSelectable(bool value)
    {
        interactableNova.enabled = value;
        if (value)
        {
            StartColorCycle();
        }
        else if (colorCoroutine != null)
        {
            StopColorCycle();
        }
    }

    public void SetSpaceName(string spaceName)
    {
        namePlate.Text = spaceName;
    }

    public void SetSpaceTexture(Texture2D texture)
    {
        image = texture;
        spaceGraphicPanel.SetImage(texture);
    }

    #region HighlightAndSelectable
    public void SetBackingShadowColor(Color color)
    {
        spaceBackingPanel.Shadow.Color = color;
    }

    [ContextMenu("StartColorCycle")]
    public void StartColorCycle()
    {
        spaceBackingPanel.Shadow.Enabled = true;
        colorCoroutine ??= StartCoroutine(ColorCycleCoroutine());
    }

    [ContextMenu("StopColorCycle")]
    public void StopColorCycle()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
            colorCoroutine = null;
            spaceBackingPanel.Shadow.Enabled = false;
        }
    }

    private IEnumerator ColorCycleCoroutine()
    {
        while (true)
        {
            Color color = RainbowColorCycler.GetColor();
            SetBackingShadowColor(color);
            yield return null;
        }
    }
    #endregion

    public void AddPlayerToSpace(Player player)
    {
        playersAtSpace.Add(player);
        player.currentSpace = this;
        GameObject piece = Instantiate(playerPiecePrefab, playerPieceLocation);
        piece.GetComponent<PlayerPiece>().player = player;
        playersPieces.Add(piece);
    }

    public void RemovePlayerFromSpace(Player player)
    {
        playersAtSpace.Remove(player);
        foreach (GameObject piece in playersPieces)
        {
            if (piece.GetComponent<PlayerPiece>().player.Equals(player))
            {
                playersPieces.Remove(piece);
                Destroy(piece);
                return;
            }
        }
    }

    public void ShowActiveCharacter(Player player)
    {
        // delete and re add token to be last in hierarchy which is displayed on top
        if (playersAtSpace.Contains(player))
        {
            RemovePlayerFromSpace(player);
            AddPlayerToSpace(player);
        }

        foreach (GameObject item in playersPieces)
        {
            item.GetComponent<PlayerPiece>().ActivePlayerEffects(player);
        }
    }

    public void AddMonsterToSpace(Monster monster)
    {
        monsterAtSpace = monster;
        hasMonster = true;
        monster.currentSpace = this;
        GameObject piece = Instantiate(monsterPiecePrefab, monsterPieceLocation);
        piece.GetComponent<MonsterPiece>().monster = monster;
        monsterPieceAtSpace = piece;
        IsBlocking = true;
    }

    public void RemoveMonsterFromSpace()
    {
        monsterAtSpace = null;
        hasMonster = false;
        Destroy(monsterPieceAtSpace);
        monsterPieceAtSpace = null;
        IsBlocking = false;
    }

    public List<IFightable> GetFightableEntities(Player playerFighting)
    {
        List<IFightable> returnable = new();

        // Add Monster
        if (hasMonster) { returnable.Add(monsterAtSpace); }

        returnable.AddRange(GetFightablePlayers(playerFighting));

        return returnable;
    }

    public List<Player> GetFightablePlayers(Player playerFighting)
    {
        List<Player> returnable = new();

        // Add Valid Players
        foreach (Player player in playersAtSpace)
        {
            if (player.Equals(playerFighting)) { continue; }

            if (playerFighting.TeamID == 0)
            {
                returnable.Add(player);
                continue;
            }

            if (playerFighting.TeamID != player.TeamID) { returnable.Add(player); }
        }

        return returnable;
    }
}