using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattleHandler : MonoBehaviour
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
        TurnState.EndBattle += HandleEndBattle;
    }

    private void OnDisable()
    {
        TurnState.EndBattle -= HandleEndBattle;
    }

    private void HandleEndBattle(TurnActor actor)
    {
        // Handle EndBattle event here
    }
}
