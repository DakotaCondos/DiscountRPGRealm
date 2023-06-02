using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMovementHandler : MonoBehaviour
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
        TurnState.EndMovement += HandleEndMovement;
    }

    private void OnDisable()
    {
        TurnState.EndMovement -= HandleEndMovement;
    }

    private void HandleEndMovement(TurnActor actor, Space space)
    {
        actor.player.currentSpace.RemovePlayerFromSpace(actor.player);
        space.AddPlayerToSpace(actor.player);

        // stop all space movement effects
        foreach (Space item in gameBoard.allSpaces)
        {
            item.TriggerSelectable(false);
        }

        // update player hasMoved
        actor.player.hasMoved = true;
        actionsManager.DetermineActions(actor);
        statDisplay.DisplayStats(actor.player.GetPower(), actor.player.GetMovement());

    }
}
