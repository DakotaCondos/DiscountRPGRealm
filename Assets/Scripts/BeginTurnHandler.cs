using System.Threading.Tasks;
using UnityEngine;

public class BeginTurnHandler : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    TurnManager turnManager;
    MonsterManager monsterManager;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        actionsManager = FindObjectOfType<ActionsManager>();
        turnManager = FindObjectOfType<TurnManager>();
        monsterManager = FindObjectOfType<MonsterManager>();
    }

    private void OnEnable()
    {
        TurnState.BeginTurn += HandleBeginTurn;
        TurnState.MonsterTurn += HandleMonsterTurn;
    }

    private void OnDisable()
    {
        TurnState.BeginTurn -= HandleBeginTurn;
        TurnState.MonsterTurn -= HandleMonsterTurn;

    }

    private void HandleBeginTurn(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleBeginTurn({actor.player.PlayerName})", Color.cyan); }

        if (!actor.isPlayer) { TurnState.TriggerMonsterTurn(actor); return; }
        actionsManager.panelSwitcher.SetActivePanel(actionsManager.mainPanel);

        actor.player.hasMoved = false;
        actionsManager.DetermineActions(actor);
        Vector3 playerSpacePos = actor.player.currentSpace.transform.position;
        Camera.main.transform.position = new Vector3(playerSpacePos.x, playerSpacePos.y, Camera.main.transform.position.z);

        // move the monsters
        actionsManager.DetermineActions(actor);
    }

    private async void HandleMonsterTurn(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleMonsterTurn({actor.player.PlayerName})", Color.cyan); }

        actionsManager.DisableButtons();
        actionsManager.panelSwitcher.SetActivePanel(actionsManager.mainPanel);
        // do monster stuff
        await monsterManager.ProcessMonsterTurn();
        await Task.Delay(500);
        // end turn
        turnManager.NextTurn();
    }
}