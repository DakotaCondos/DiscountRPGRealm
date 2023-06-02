using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMovementHandler : MonoBehaviour
{
    GameBoard gameBoard;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
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
        // remove from current space
        actor.player.currentSpace.RemovePlayerFromSpace(actor.player);
        // add to new space
        space.AddPlayerToSpace(actor.player);
        // stop all space movement effects
        foreach (Space item in gameBoard.spaces)
        {
            item.TriggerSelectable(false);
        }
        // update player hasMoved
        actor.player.hasMoved = true;
    }
}
