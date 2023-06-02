using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginBattleHandler : MonoBehaviour
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
        TurnState.BeginBattle += HandleBeginBattle;
    }

    private void OnDisable()
    {
        TurnState.BeginBattle -= HandleBeginBattle;
    }

    private void HandleBeginBattle(TurnActor actor)
    {
        // Handle BeginBattle event here
    }
}
