using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginMovementHandler : MonoBehaviour
{
    GameBoard gameBoard;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
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
        }
        else
        {

        }

    }
}
