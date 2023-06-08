using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BeginBattleHandler : MonoBehaviour
{
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
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleBeginBattlePvP({player.PlayerName}, {opponent.PlayerName})", Color.cyan); }


        CombatManager.Instance.CreateEncounter(player, opponent);
    }

    private void HandleBeginBattlePvM(Player player, Monster monster)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleBeginBattlePvM({player.PlayerName}, {monster.MonsterName})", Color.cyan); }

        CombatManager.Instance.CreateEncounter(player, monster);
    }
}
