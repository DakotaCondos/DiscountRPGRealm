using UnityEngine;

public class EndTurnHandler : MonoBehaviour
{
    TeleportManager teleportManager;
    TurnOrderPanel turnOrderPanel;

    private void Awake()
    {
        teleportManager = FindObjectOfType<TeleportManager>(true);
        turnOrderPanel = FindObjectOfType<TurnOrderPanel>(true);
    }

    private void OnEnable()
    {
        TurnState.EndTurn += HandleEndTurn;
    }

    private void OnDisable()
    {
        TurnState.EndTurn -= HandleEndTurn;
    }

    public void HandleEndTurn(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndTurn({actor.player.PlayerName})", Color.cyan); }

        teleportManager.EvaluateSpaces(actor.player);
        turnOrderPanel.UpdatePanel();
    }
}
