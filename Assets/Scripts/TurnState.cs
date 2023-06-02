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
    BeginBattle,
    EndBattle,
    BeginChallenge,
    EndChallenge
}

public class TurnState : MonoBehaviour
{
    public static event Action StartGame;
    public static event Action EndGame;
    public static event Action<TurnActor> BeginTurn;
    public static event Action<TurnActor> EndTurn;
    public static event Action<TurnActor> BeginMovement;
    public static event Action<TurnActor, Space> EndMovement;
    public static event Action<TurnActor> BeginBattle;
    public static event Action<TurnActor> EndBattle;
    public static event Action<TurnActor> BeginChallenge;
    public static event Action<TurnActor> EndChallenge;

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

    public static void TriggerBeginBattle(TurnActor actor)
    {
        turnStage = TurnStages.BeginBattle;
        BeginBattle?.Invoke(actor);
    }

    public static void TriggerEndBattle(TurnActor actor)
    {
        turnStage = TurnStages.EndBattle;
        EndBattle?.Invoke(actor);
    }

    public static void TriggerBeginChallenge(TurnActor actor)
    {
        turnStage = TurnStages.BeginChallenge;
        BeginChallenge?.Invoke(actor);
    }

    public static void TriggerEndChallenge(TurnActor actor)
    {
        turnStage = TurnStages.EndChallenge;
        EndChallenge?.Invoke(actor);
    }
}
