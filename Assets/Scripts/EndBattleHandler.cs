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

    private async void HandleEndBattlePvP(Player player, Player opponent, bool won)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndBattlePvP({player.PlayerName}, {opponent.PlayerName}, {won})", Color.cyan); }

        Player winner = (won) ? player : opponent;
        Player loser = (!won) ? player : opponent;
        HandlePVP(loser, winner);
        if (won) { actionsManager.SetHasFought(true); }

        print("Starting PlayerEffectsHandler from EndBattleHandler");
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.playerEffectsPanel);
        TaskHelper helper = new();
        StartCoroutine(PlayerEffectsHandler.Instance.HandleEffects(helper));

        while (!helper.isComplete)
        {
            await Task.Delay(100);
        }
        print("Ended PlayerEffectsHandler from EndBattleHandler");

        if (won)
        {
            actionsManager.DetermineActions(TurnManager.Instance.GetCurrentActor());
        }
        else
        {
            TurnManager.Instance.NextTurn();
        }
    }

    private async void HandleEndBattlePvM(Player player, Monster monster, bool won)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleEndBattlePvM({player.PlayerName}, {monster.MonsterName}, {won})", Color.cyan); }
        if (won)
        {
            MonsterManager.Instance.KillMonster(monster, player);
            actionsManager.SetHasFought(true);
        }
        else
        {
            int moneyLost = (player.money / 5);
            player.effects.Enqueue(new(PlayerEffectType.Money, -moneyLost));
            MovePlayerToStart(player);
            MonsterManager.Instance.ScaleMonsterPower(monster);
        }

        print("Starting PlayerEffectsHandler from EndBattleHandler");
        TaskHelper helper = new();
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.playerEffectsPanel);
        StartCoroutine(PlayerEffectsHandler.Instance.HandleEffects(helper));

        while (!helper.isComplete)
        {
            await Task.Delay(100);
        }
        print("Ended PlayerEffectsHandler from EndBattleHandler");

        if (won)
        {
            actionsManager.DetermineActions(TurnManager.Instance.GetCurrentActor());
        }
        else
        {
            TurnManager.Instance.NextTurn();
        }
    }

    private void HandlePVP(Player losingPlayer, Player WinningPlayer)
    {
        int moneyLost = losingPlayer.money / 2;
        if (moneyLost != 0)
        {
            losingPlayer.effects.Enqueue(new(PlayerEffectType.Money, -moneyLost));
            WinningPlayer.effects.Enqueue(new(PlayerEffectType.Money, moneyLost));
        }
        MovePlayerToStart(losingPlayer);
    }

    private void MovePlayerToStart(Player player)
    {
        // move player
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"MovePlayerToStart({player.PlayerName}", Color.cyan); }

        Space startingSpace = GameBoard.Instance.allSpaces.FirstOrDefault(space => space.isStartingSpace == true);
        player.currentSpace.RemovePlayerFromSpace(player);
        startingSpace.AddPlayerToSpace(player);
    }
}
