using System.Collections;
using System.Collections.Generic;
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
        // Handle EndTurn event here
        turnOrderPanel.UpdatePanel();
    }
}
