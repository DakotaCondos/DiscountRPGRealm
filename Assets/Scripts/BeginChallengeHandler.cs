using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginChallengeHandler : MonoBehaviour
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
        TurnState.BeginChallenge += HandleBeginChallenge;
    }

    private void OnDisable()
    {
        TurnState.BeginChallenge -= HandleBeginChallenge;
    }

    private void HandleBeginChallenge(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleBeginChallenge({actor.player.PlayerName})", Color.cyan); }

        // Handle BeginChallenge event here
    }
}
