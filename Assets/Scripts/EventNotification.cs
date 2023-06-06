using System;
using UnityEngine;

public class EventNotification : MonoBehaviour
{
    // development script 

    private void OnEnable()
    {
        TurnState.StartGame += OnStartGame;
        TurnState.EndGame += OnEndGame;
        TurnState.BeginTurn += OnBeginTurn;
        TurnState.EndTurn += OnEndTurn;
        TurnState.BeginMovement += OnBeginMovement;
        TurnState.EndMovement += OnEndMovement;
        TurnState.BeginBattlePvP += OnBeginBattlePvP;
        TurnState.BeginBattlePvM += OnBeginBattlePvM;
        TurnState.EndBattlePvP += OnEndBattlePvP;
        TurnState.EndBattlePvM += OnEndBattlePvM;
        TurnState.BeginChallenge += OnBeginChallenge;
        TurnState.EndChallenge += OnEndChallenge;
    }

    private void OnDisable()
    {
        TurnState.StartGame -= OnStartGame;
        TurnState.EndGame -= OnEndGame;
        TurnState.EndTurn -= OnEndTurn;
        TurnState.BeginMovement -= OnBeginMovement;
        TurnState.EndMovement -= OnEndMovement;
        TurnState.BeginBattlePvP -= OnBeginBattlePvP;
        TurnState.BeginBattlePvM -= OnBeginBattlePvM;
        TurnState.EndBattlePvP -= OnEndBattlePvP;
        TurnState.EndBattlePvM -= OnEndBattlePvM;
        TurnState.BeginChallenge -= OnBeginChallenge;
        TurnState.EndChallenge -= OnEndChallenge;
    }
    private void OnStartGame()
    {
        Debug.Log($"Notification StartGame");
    }

    private void OnEndGame()
    {
        Debug.Log($"Notification EndGame");
    }

    private void OnBeginTurn(TurnActor actor)
    {
        Debug.Log($"Notification BeginTurn: {actor.player.PlayerName}");
    }

    private void OnEndTurn(TurnActor actor)
    {
        Debug.Log($"Notification EndTurn: {actor.player.PlayerName}");
    }

    private void OnBeginMovement(TurnActor actor)
    {
        Debug.Log($"Notification BeginMovement: {actor.player.PlayerName}");
    }

    private void OnEndMovement(TurnActor actor, Space space)
    {
        Debug.Log($"Notification EndMovement: {actor.player.PlayerName}");
    }

    private void OnBeginBattlePvP(Player player, Player oponent)
    {
        Debug.Log($"Notification BeginBattlePvP: {player.PlayerName} vs. {oponent.PlayerName}");
    }

    private void OnBeginBattlePvM(Player player, Monster oponent)
    {
        Debug.Log($"Notification BeginBattlePvM: {player.PlayerName} vs. {oponent.MonsterName}");
    }

    private void OnEndBattlePvP(Player player, Player oponent, bool won)
    {
        Debug.Log($"Notification EndBattlePvP: {player.PlayerName} vs. {oponent.PlayerName}");
    }

    private void OnEndBattlePvM(Player player, Monster oponent, bool won)
    {
        Debug.Log($"Notification EndBattlePvM: {player.PlayerName} vs. {oponent.MonsterName}");
    }

    private void OnBeginChallenge(TurnActor actor)
    {
        Debug.Log($"Notification BeginChallenge: {actor.player.PlayerName}");
    }

    private void OnEndChallenge(TurnActor actor)
    {
        Debug.Log($"Notification EndChallenge: {actor.player.PlayerName}");
    }
}
