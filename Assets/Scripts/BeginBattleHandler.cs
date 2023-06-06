using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BeginBattleHandler : MonoBehaviour
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
        TurnState.BeginBattlePvP += HandleBeginBattlePvP;
        TurnState.BeginBattlePvM += HandleBeginBattlePvM;
    }

    private void OnDisable()
    {
        TurnState.BeginBattlePvP -= HandleBeginBattlePvP;
        TurnState.BeginBattlePvM -= HandleBeginBattlePvM;
    }

    private void HandleBeginBattlePvP(Player player, Player opponent)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleBeginBattlePvP({player.PlayerName}, {opponent.PlayerName})", Color.cyan); }


        CombatManager.Instance.CreateEncounter(player, opponent);
    }

    private void HandleBeginBattlePvM(Player player, Monster monster)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleBeginBattlePvM({player.PlayerName}, {monster.MonsterName})", Color.cyan); }

        CombatManager.Instance.CreateEncounter(player, monster);
    }
}
