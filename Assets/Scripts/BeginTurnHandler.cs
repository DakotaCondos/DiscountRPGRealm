using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnHandler : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    StatDisplay statDisplay;
    TurnManager turnManager;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        actionsManager = FindObjectOfType<ActionsManager>();
        turnManager = FindObjectOfType<TurnManager>();
        statDisplay = FindObjectOfType<StatDisplay>();
    }

    private void OnEnable()
    {
        TurnState.BeginTurn += HandleBeginTurn;
        TurnState.MonsterTurn += HandleMonsterTurn;
    }

    private void OnDisable()
    {
        TurnState.BeginTurn -= HandleBeginTurn;
        TurnState.MonsterTurn -= HandleMonsterTurn;

    }

    private void HandleBeginTurn(TurnActor actor)
    {
        if (!actor.isPlayer) { TurnState.TriggerMonsterTurn(actor); return; }

        actor.player.hasMoved = false;
        actionsManager.DetermineActions(actor);
        statDisplay.DisplayStats(actor.player.GetPower(), actor.player.GetMovement());
        Vector3 playerSpacePos = actor.player.currentSpace.transform.position;
        Camera.main.transform.position = new Vector3(playerSpacePos.x, playerSpacePos.y, Camera.main.transform.position.z);

        // move the monsters
        actionsManager.DetermineActions(actor);


        foreach (Space space in gameBoard.allSpaces)
        {
            space.ShowActiveCharacter(actor.player);
        }
    }

    private void HandleMonsterTurn(TurnActor actor)
    {
        Debug.LogWarning("Monster Turn Goes Here");
        // do monster stuff

        // end turn
        turnManager.NextTurn();
    }
}