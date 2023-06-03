using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginMovementHandler : MonoBehaviour
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
        TurnState.BeginMovement += HandleBeginMovement;
    }

    private void OnDisable()
    {
        TurnState.BeginMovement -= HandleBeginMovement;
    }

    private void HandleBeginMovement(TurnActor actor)
    {
        if (actor.isPlayer)
        {
            Space current = actor.player.currentSpace;

            List<Space> availableSpaces = gameBoard.SpacesInRange(current, actor.player.GetMovement());
            // apply movement effects
            foreach (Space space in availableSpaces)
            {
                space.TriggerSelectable(true);
            }

            actionsManager.DetermineActions(actor);
            statDisplay.DisplayStats(actor.player.GetPower(), actor.player.GetMovement());
        }
        else
        {
            // Foreach monster, if moveable has x% chance to move to adjecent space that is not blocked
            // unblock current space and block moved to space (if any)
        }

    }
}
