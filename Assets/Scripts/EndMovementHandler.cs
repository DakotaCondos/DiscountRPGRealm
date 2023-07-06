using System.Threading.Tasks;
using UnityEngine;

public class EndMovementHandler : SceneSingleton<EndMovementHandler>
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    private new void Awake()
    {
        base.Awake();
        gameBoard = FindObjectOfType<GameBoard>();
        actionsManager = FindObjectOfType<ActionsManager>();
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
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndMovement({actor.player.PlayerName})", Color.cyan); }

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
