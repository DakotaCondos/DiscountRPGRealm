using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndChallengeHandler : MonoBehaviour
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
        TurnState.EndChallenge += HandleEndChallenge;
    }

    private void OnDisable()
    {
        TurnState.EndChallenge -= HandleEndChallenge;
    }

    private void HandleEndChallenge(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleEndChallenge({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here

        actionsManager.DetermineActions(actor);
    }
}
