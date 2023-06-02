using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnHandler : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        actionsManager = FindObjectOfType<ActionsManager>();
    }

    private void OnEnable()
    {
        TurnState.BeginTurn += HandleBeginTurn;
    }

    private void OnDisable()
    {
        TurnState.BeginTurn -= HandleBeginTurn;
    }

    private void HandleBeginTurn(TurnActor actor)
    {
        if (gameBoard == null) { Debug.LogWarning("BeginTurnHandler could not find GameBoard"); }
        // Handle BeginTurn event here
        if (actor.isPlayer)
        {
            actor.player.hasMoved = false;
            actionsManager.DetermineActions(actor);
        }
        else
        {
            // move the monsters
        }

        foreach (Space space in gameBoard.spaces)
        {
            space.ShowActiveCharacter(actor.player);
        }
    }
}
