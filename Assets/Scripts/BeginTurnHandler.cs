using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BeginTurnHandler : MonoBehaviour
{
    GameBoard gameBoard;
    ActionsManager actionsManager;
    TurnManager turnManager;
    MonsterManager monsterManager;
    GameDifficulty currentGameDifficulty;
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

    private void Start()
    {
        currentGameDifficulty = GameManager.Instance.gameDifficulty;
    }

    private async void HandleBeginTurn(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleBeginTurn({actor.player.PlayerName})", Color.cyan); }

        await AnimationPanelManager.Instance.BeginTurnAnimation(actor);

        if (!actor.isPlayer) { TurnState.TriggerMonsterTurn(actor); return; }
        actionsManager.panelSwitcher.SetActivePanel(actionsManager.mainPanel);
        ApplyPlayerXP(actor.player);

        actor.player.hasMoved = false;
        actionsManager.DetermineActions(actor);


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

        // Difficulty Modifiers
        ApplyPlayerXPCurve();

        // end turn
        turnManager.NextTurn();
    }

    private void ApplyPlayerXPCurve()
    {
        switch (currentGameDifficulty)
        {
            case GameDifficulty.Relaxed:
                CurveXP(0.2f);
                break;
            case GameDifficulty.Easy:
                CurveXP(0.1f);
                break;
            case GameDifficulty.Normal:
                CurveXP(0.1f);
                break;
            case GameDifficulty.Hard:
                CurveXP(0.05f);
                break;
            case GameDifficulty.Insane:
                break;
        }
    }

    private void CurveXP(float value)
    {
        List<Player> humanPlayers = new();

        int totalXP = 0;
        foreach (Player player in GameManager.Instance.Players)
        {
            if (player.TeamID != 7)
            {
                humanPlayers.Add(player);
                totalXP += player.xp;
            }
        }
        int avgXP = totalXP / humanPlayers.Count;

        foreach (Player player in humanPlayers)
        {
            int xpDelta = avgXP - player.xp;
            if (xpDelta <= 0) { continue; }
            player.xp += Mathf.Clamp((int)(xpDelta * value), 1, 10);
        }
    }

    private void ApplyPlayerXP(Player player)
    {
        switch (currentGameDifficulty)
        {
            case GameDifficulty.Relaxed:
                player.AddXP(3);
                break;
            case GameDifficulty.Easy:
                player.AddXP(2);
                break;
            case GameDifficulty.Normal:
                player.AddXP(1);
                break;
            case GameDifficulty.Hard:
                break;
            case GameDifficulty.Insane:
                break;
            default:
                break;
        }
    }
}