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
    Trap
}

public class Space : MonoBehaviour
{
    // Setup Variable
    public SpaceType spaceType;
    public bool canMonstersTraverse = true;
    public bool canSpawnMonsters = true;
    [SerializeField] bool isStartingSpace = false;
    public List<Space> ConnectedSpaces = new List<Space>();

    // Setup Static
    public GameObject lineDrawPoint;
    public GameObject monsterPiecePrefab;
    public Transform monsterPieceLocation;
    public UIBlock2D spaceBackingPanel;
    public GameObject playerPiecePrefab;
    public Transform playerPieceLocation;
    public TextBlock namePlate;

    // Current State
    public Monster monsterAtSpace;
    public GameObject monsterPieceAtSpace;
    public List<Player> playersAtSpace;
    public List<GameObject> playersPieces = new();
    public bool IsBlocking = false;

    // Other
    private Coroutine colorCoroutine; // Reference to the coroutine

    private void Awake()
    {
        playersAtSpace = new();
    }

    public void Initialize()
    {
        // Draw Lines to connected Spaces
        LineDrawer lineDrawer = FindObjectOfType<LineDrawer>();
        if (lineDrawer == null)
        {
            Debug.LogWarning($"{name} could not find LineDrawer");
        }
        else
        {
            foreach (Space space in ConnectedSpaces)
            {
                lineDrawer.DrawLine(lineDrawPoint, space.lineDrawPoint);
            }
        }



        // Set Players at starting space
        if (!isStartingSpace) return;
        TurnManager tm = FindObjectOfType<TurnManager>();
        foreach (TurnActor turnActor in tm.GetUpcomingPlayers())
        {
            if (turnActor.isPlayer)
            {
                AddPlayerToSpace(turnActor.player);
            }
        }
        AddPlayerToSpace(tm.GetCurrentPlayer().player);
        //ShowActiveCharacter(tm.GetCurrentPlayer().player);
    }

    public void SelectSpace()
    {
        // Code to handle space selection here.
        print($"selected {gameObject.name}");
        if (colorCoroutine != null)
        {
            StopColorCycle();
        }
        else
        {
            StartColorCycle();
        }
    }

    public void SetSpaceBlocked(bool value)
    {
        IsBlocking = value;
    }

    public void SetSpaceName(string spaceName)
    {
        namePlate.Text = spaceName;
    }

    #region ShadowColor
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
        GameObject piece = Instantiate(monsterPiecePrefab, monsterPieceLocation);
        piece.GetComponent<MonsterPiece>().monster = monster;
        monsterPieceAtSpace = piece;
    }

    public void RemoveMonsterFromSpace()
    {
        monsterAtSpace = null;
        Destroy(monsterPieceAtSpace);
        monsterPieceAtSpace = null;
    }
}