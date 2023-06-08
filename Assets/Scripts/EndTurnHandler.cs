using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EndTurnHandler : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    StatDisplay statDisplay;
    TurnOrderPanel turnOrderPanel;

    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        actionsManager = FindObjectOfType<ActionsManager>();
        statDisplay = FindObjectOfType<StatDisplay>();
        turnOrderPanel = FindObjectOfType<TurnOrderPanel>(true);
    }

    private void OnEnable()
    {
        TurnState.EndTurn += HandleEndTurn;
    }

    private void OnDisable()
    {
        TurnState.EndTurn -= HandleEndTurn;
    }

    public void HandleEndTurn(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndTurn({actor.player.PlayerName})", Color.cyan); }

        turnOrderPanel.UpdatePanel();
    }
}
