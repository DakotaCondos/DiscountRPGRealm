using System;
using UnityEngine;

public class TurnState : MonoBehaviour
{
    public static event Action<TurnActor> BeginTurn;
    public static event Action<TurnActor> EndTurn;
    public static event Action<TurnActor> BeginMovement;
    public static event Action<TurnActor> EndMovement;
    public static event Action<TurnActor> BeginBattle;
    public static event Action<TurnActor> EndBattle;
    public static event Action<TurnActor> BeginChallenge;
    public static event Action<TurnActor> EndChallenge;

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

    public static void TriggerEndMovement(TurnActor actor)
    {
        EndMovement?.Invoke(actor);
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