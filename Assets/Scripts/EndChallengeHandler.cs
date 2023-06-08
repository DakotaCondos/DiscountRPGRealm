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

    private void HandleEndChallenge(TurnActor actor, bool loseTurn, bool idkYet)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleEndChallenge({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.mainPanel);

        if (loseTurn) { TurnManager.Instance.NextTurn(); }

        // Update Actions
        ActionsManager.Instance.SetHasInteracted(true);
        ActionsManager.Instance.DetermineActions(actor);
    }
}
