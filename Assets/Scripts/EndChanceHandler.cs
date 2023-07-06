using UnityEngine;

public class EndChanceHandler : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    StatDisplay statDisplay;
    CameraController cameraController;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        actionsManager = FindObjectOfType<ActionsManager>();
        statDisplay = FindObjectOfType<StatDisplay>();
        cameraController = FindObjectOfType<CameraController>();
    }
    private void OnEnable()
    {
        TurnState.EndChance += HandleEndChance;
    }

    private void OnDisable()
    {
        TurnState.EndChance -= HandleEndChance;
    }

    private void HandleEndChance(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndChance({actor.player.PlayerName})", Color.cyan); }
        // Handle EndChallenge event here
        actionsManager.SetHasInteracted(true);
        actionsManager.panelSwitcher.SetActivePanel(actionsManager.mainPanel);
        cameraController.snapToOutOfBoundsView = false;
        actionsManager.DetermineActions(actor);
    }
}