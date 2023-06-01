using System;
using UnityEngine;

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

    public static void TriggerStartGame()
    {
        StartGame?.Invoke();
    }
    public static void TriggerEndGame()
    {
        EndGame?.Invoke();
    }
    public static void TriggerBeginTurn(TurnActor actor)
    {
        BeginTurn?.Invoke(actor);
    }

    public static void TriggerEndTurn(TurnActor actor)
    {
        EndTurn?.Invoke(actor);
    }

    public static void TriggerBeginMovement(TurnActor actor)
    {
        BeginMovement?.Invoke(actor);
    }

    public static void TriggerEndMovement(TurnActor actor, Space space)
    {
        EndMovement?.Invoke(actor, space);
    }

    public static void TriggerBeginBattle(TurnActor actor)
    {
        BeginBattle?.Invoke(actor);
    }

    public static void TriggerEndBattle(TurnActor actor)
    {
        EndBattle?.Invoke(actor);
    }

    public static void TriggerBeginChallenge(TurnActor actor)
    {
        BeginChallenge?.Invoke(actor);
    }

    public static void TriggerEndChallenge(TurnActor actor)
    {
        EndChallenge?.Invoke(actor);
    }
}
