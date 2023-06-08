using System;
using UnityEngine;

public enum TurnStages
{
    StartGame,
    EndGame,
    BeginTurn,
    EndTurn,
    BeginMovement,
    EndMovement,
    BeginBattlePvP,
    BeginBattlePvM,
    EndBattlePvP,
    EndBattlePvM,
    BeginChallenge,
    EndChallenge,
    BeginChance,
    EndChance,
    MonsterTurn
}

public class TurnState : MonoBehaviour
{
    public static event Action StartGame;
    public static event Action EndGame;
    public static event Action<TurnActor> BeginTurn;
    public static event Action<TurnActor> MonsterTurn;
    public static event Action<TurnActor> EndTurn;
    public static event Action<TurnActor> BeginMovement;
    public static event Action<TurnActor, Space> EndMovement;
    public static event Action<Player, Player> BeginBattlePvP;
    public static event Action<Player, Monster> BeginBattlePvM;
    public static event Action<Player, Player, bool> EndBattlePvP;
    public static event Action<Player, Monster, bool> EndBattlePvM;
    public static event Action<TurnActor> BeginChallenge;
    public static event Action<TurnActor, bool, bool> EndChallenge; // TurnActor actor, bool loseTurn, bool ???>
    public static event Action<TurnActor> BeginChance;
    public static event Action<TurnActor> EndChance;

    private static TurnStages turnStage;

    public static TurnStages TurnStage { get => turnStage; }

    public static void TriggerStartGame()
    {
        turnStage = TurnStages.StartGame;
        StartGame?.Invoke();
    }

    public static void TriggerEndGame()
    {
        turnStage = TurnStages.EndGame;
        EndGame?.Invoke();
    }

    public static void TriggerBeginTurn(TurnActor actor)
    {
        turnStage = TurnStages.BeginTurn;
        BeginTurn?.Invoke(actor);
    }
    public static void TriggerMonsterTurn(TurnActor actor)
    {
        turnStage = TurnStages.MonsterTurn;
        MonsterTurn?.Invoke(actor);
    }

    public static void TriggerEndTurn(TurnActor actor)
    {
        turnStage = TurnStages.EndTurn;
        EndTurn?.Invoke(actor);
    }

    public static void TriggerBeginMovement(TurnActor actor)
    {
        turnStage = TurnStages.BeginMovement;
        BeginMovement?.Invoke(actor);
    }

    public static void TriggerEndMovement(TurnActor actor, Space space)
    {
        turnStage = TurnStages.EndMovement;
        EndMovement?.Invoke(actor, space);
    }

    public static void TriggerBeginBattlePvP(Player player, Player opponent)
    {
        print($"TriggerBeginBattlePvP called {player.PlayerName} vs. {opponent.PlayerName}");
        turnStage = TurnStages.BeginBattlePvP;
        BeginBattlePvP?.Invoke(player, opponent);
    }

    public static void TriggerBeginBattlePvM(Player player, Monster monster)
    {
        turnStage = TurnStages.BeginBattlePvM;
        BeginBattlePvM?.Invoke(player, monster);
    }

    public static void TriggerEndBattlePvP(Player player, Player opponent, bool win)
    {
        turnStage = TurnStages.EndBattlePvP;
        EndBattlePvP?.Invoke(player, opponent, win);
    }

    public static void TriggerEndBattlePvM(Player player, Monster monster, bool win)
    {
        turnStage = TurnStages.EndBattlePvM;
        EndBattlePvM?.Invoke(player, monster, win);
    }

    public static void TriggerBeginChallenge(TurnActor actor)
    {
        turnStage = TurnStages.BeginChallenge;
        BeginChallenge?.Invoke(actor);
    }

    public static void TriggerEndChallenge(TurnActor actor, bool loseTurn, bool idkYet)
    {
        turnStage = TurnStages.EndChallenge;
        EndChallenge?.Invoke(actor, loseTurn, idkYet);
    }

    public static void TriggerBeginChance(TurnActor actor)
    {
        turnStage = TurnStages.BeginChance;
        BeginChance?.Invoke(actor);
    }

    public static void TriggerEndChance(TurnActor actor)
    {
        turnStage = TurnStages.EndChance;
        EndChance?.Invoke(actor);
    }
}
