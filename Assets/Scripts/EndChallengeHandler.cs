using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    private async void HandleEndChallenge(TurnActor actor, bool endTurn, bool idkYet)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndChallenge({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here
        CameraController.Instance.snapToOutOfBoundsView = false;

        if (!endTurn)
        {
            print("Starting PlayerEffectsHandler from GuessingGame");
            TaskHelper helper = new();
            ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.playerEffectsPanel);
            StartCoroutine(PlayerEffectsHandler.Instance.HandleEffects(helper));
            while (!helper.isComplete)
            {
                await Task.Delay(100);
            }
        }

        if (endTurn)
        {
            TurnManager.Instance.NextTurn();
        }
        else
        {
            ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.mainPanel);
            // Update Actions
            ActionsManager.Instance.SetHasInteracted(true);
            ActionsManager.Instance.DetermineActions(actor);
        }
    }
}
