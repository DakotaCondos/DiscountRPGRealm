using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum SpaceType
{
    Empty,
    Town,
    Chance,
    Event,
    MonsterSpawn,
    Challenge,
    Custom
}

public class Space : MonoBehaviour
{
    // Setup Variable
    public SpaceType spaceType;
    public bool canMonstersTraverse = true;
    public bool canSpawnMonsters = true;
    public int shopLevel = 0;
    public bool hasMandatoryEvent = false;
    public bool isStartingSpace = false;
    public List<Space> ConnectedSpaces = new List<Space>();
    public SpaceSO blueprint = null;

    // Setup Static
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

    // Current State
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