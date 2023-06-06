using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EndBattleHandler : MonoBehaviour
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
        TurnState.EndBattlePvP += HandleEndBattlePvP;
        TurnState.EndBattlePvM += HandleEndBattlePvM;
    }

    private void OnDisable()
    {
        TurnState.EndBattlePvP -= HandleEndBattlePvP;
        TurnState.EndBattlePvM -= HandleEndBattlePvM;
    }

    private void HandleEndBattlePvP(Player player, Player opponent, bool won)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleEndBattlePvP({player.PlayerName}, {opponent.PlayerName}, {won})", Color.cyan); }

        Player winner = (won) ? player : opponent;
        Player loser = (!won) ? player : opponent;
        HandlePVP(loser, winner);
        if (won)
        {
            actionsManager.SetHasFought(true);
            actionsManager.DetermineActions(TurnManager.Instance.GetCurrentActor());
        }
        else
        {
            TurnManager.Instance.NextTurn();
        }
    }

    private void HandleEndBattlePvM(Player player, Monster monster, bool won)
    {
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"HandleEndBattlePvM({player.PlayerName}, {monster.MonsterName}, {won})", Color.cyan); }

        if (won)
        {
            MonsterManager.Instance.KillMonster(monster, player);
            actionsManager.SetHasFought(true);
            actionsManager.DetermineActions(TurnManager.Instance.GetCurrentActor());
        }
        else
        {
            int moneyLost = (player.money / 5);
            player.AddMoney(-moneyLost);
            MovePlayerToStart(player);
            TurnManager.Instance.NextTurn();
        }
    }

    private void HandlePVP(Player losingPlayer, Player WinningPlayer)
    {
        int moneyLost = losingPlayer.money / 2;
        losingPlayer.AddMoney(-moneyLost);
        WinningPlayer.AddMoney(moneyLost);
        MovePlayerToStart(losingPlayer);
    }

    private void MovePlayerToStart(Player player)
    {
        // move player
        if (ApplicationManager.Instance.handlerNotifications) { ConsolePrinter.PrintToConsole($"MovePlayerToStart({player.PlayerName}", Color.cyan); }

        Space startingSpace = GameBoard.Instance.allSpaces.FirstOrDefault(space => space.isStartingSpace == true);
        player.currentSpace.RemovePlayerFromSpace(player);
        startingSpace.AddPlayerToSpace(player);
    }
}
