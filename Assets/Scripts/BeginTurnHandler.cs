using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnHandler : MonoBehaviour
{
    GameBoard gameBoard;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
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
            // determin actions player can take
        }
        else
        {
            // move the monsters
        }

        foreach (Space space in gameBoard.Spaces)
        {
            space.ShowActiveCharacter(actor.player);
        }
    }
}
