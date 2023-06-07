using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndChanceHandler : MonoBehaviour
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
        TurnState.EndChance += HandleEndChance;
    }

    private void OnDisable()
    {
        TurnState.EndChance -= HandleEndChance;
    }

    private void HandleEndChance(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleEndChance({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here
    }
}