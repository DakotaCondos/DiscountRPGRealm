using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginChanceHandler : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    StatDisplay statDisplay;
    CardMiniGame cardMiniGame;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>(true);
        actionsManager = FindObjectOfType<ActionsManager>(true);
        statDisplay = FindObjectOfType<StatDisplay>(true);
        cardMiniGame = FindObjectOfType<CardMiniGame>(true);
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
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleBeginChance({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here
        cardMiniGame.CreateChanceGame(actor);
    }
}
