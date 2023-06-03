using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnHandler : MonoBehaviour
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
            statDisplay.DisplayStats(actor.player.GetPower(), actor.player.GetMovement());
            Vector3 playerSpacePos = actor.player.currentSpace.transform.position;
            Camera.main.transform.position = new Vector3(playerSpacePos.x, playerSpacePos.y, Camera.main.transform.position.z);
        }
        else
        {
            // move the monsters
            actionsManager.DetermineActions(actor);
        }

        foreach (Space space in gameBoard.allSpaces)
        {
            space.ShowActiveCharacter(actor.player);
        }
    }
}