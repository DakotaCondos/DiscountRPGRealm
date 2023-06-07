using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginHandleChance : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    StatDisplay statDisplay;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        actionsManager = FindObjectOfType<ActionsManager>();
        statDisplay = FindObjectOfType<StatDisplay>();
    }
    private void OnEnable()
    {
        TurnState.BeginChance += HandleBeginChance;
    }

    private void OnDisable()
    {
        TurnState.BeginChance -= HandleBeginChance;
    }

    private void HandleBeginChance(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleBeginChance({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here
    }
}
