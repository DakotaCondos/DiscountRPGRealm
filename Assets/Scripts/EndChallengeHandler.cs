using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndChallengeHandler : MonoBehaviour
{
    private void OnEnable()
    {
        TurnState.EndChallenge += HandleEndChallenge;
    }

    private void OnDisable()
    {
        TurnState.EndChallenge -= HandleEndChallenge;
    }

    private void HandleEndChallenge(TurnActor actor, bool endTurn, bool idkYet)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndChallenge({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.mainPanel);
        CameraController.Instance.snapToOutOfBoundsView = false;

        if (endTurn)
        {
            TurnManager.Instance.NextTurn();
        }
        else
        {
            // Update Actions
            ActionsManager.Instance.SetHasInteracted(true);
            ActionsManager.Instance.DetermineActions(actor);
        }
    }
}
