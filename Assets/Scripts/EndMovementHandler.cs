using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    private async void HandleEndMovement(TurnActor actor, Space endSpace)
    {
        // stop all space movement effects
        foreach (Space item in gameBoard.allSpaces)
        {
            item.TriggerSelectable(false);
        }

        // if moving to the same space as player is occupying, consider this as 'Cancelled Movement'
        if (!endSpace.Equals(actor.player.currentSpace))
        {
            // move player
            actor.player.currentSpace.RemovePlayerFromSpace(actor.player);

            // do async game board movement stuff
            await PerformGameBoardMovementAsync(actor, endSpace);

            // wait for game board stuff to end
            endSpace.AddPlayerToSpace(actor.player);

            // update player hasMoved
            actor.player.hasMoved = true;
        }

        actionsManager.DetermineActions(actor);
        statDisplay.DisplayStats(actor.player.GetPower(), actor.player.GetMovement());
    }

    private async Task PerformGameBoardMovementAsync(TurnActor actor, Space endSpace)
    {
        TaskHelper helper = new TaskHelper();

        // Perform your game board movement logic asynchronously here
        gameBoard.actorPieceMovement.MoveActor(actor, actor.player.currentSpace, endSpace, helper);

        while (!helper.isComplete)
        {
            await Task.Delay(100); // Wait for 100 milliseconds before checking again
        }
    }
}
