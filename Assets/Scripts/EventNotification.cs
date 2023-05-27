using UnityEngine;

public class EventNotification : MonoBehaviour
{
    // development script 

    private void OnEnable()
    {
        TurnState.BeginTurn += OnBeginTurn;
        TurnState.EndTurn += OnEndTurn;
        TurnState.BeginMovement += OnBeginMovement;
        TurnState.EndMovement += OnEndMovement;
        TurnState.BeginBattle += OnBeginBattle;
        TurnState.EndBattle += OnEndBattle;
        TurnState.BeginChallenge += OnBeginChallenge;
        TurnState.EndChallenge += OnEndChallenge;
    }

    private void OnDisable()
    {
        TurnState.BeginTurn -= OnBeginTurn;
        TurnState.EndTurn -= OnEndTurn;
        TurnState.BeginMovement -= OnBeginMovement;
        TurnState.EndMovement -= OnEndMovement;
        TurnState.BeginBattle -= OnBeginBattle;
        TurnState.EndBattle -= OnEndBattle;
        TurnState.BeginChallenge -= OnBeginChallenge;
        TurnState.EndChallenge -= OnEndChallenge;
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

    private void OnEndMovement(TurnActor actor)
    {
        Debug.Log($"Notification EndMovement: {actor.player.PlayerName}");
    }

    private void OnBeginBattle(TurnActor actor)
    {
        Debug.Log($"Notification BeginBattle: {actor.player.PlayerName}");
    }

    private void OnEndBattle(TurnActor actor)
    {
        Debug.Log($"Notification EndBattle: {actor.player.PlayerName}");
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
