using UnityEngine;

public class EventNotification : MonoBehaviour
{
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
        Debug.Log($"BeginTurn: {actor.player.PlayerName}");
    }

    private void OnEndTurn(TurnActor actor)
    {
        Debug.Log($"EndTurn: {actor.player.PlayerName}");
    }

    private void OnBeginMovement(TurnActor actor)
    {
        Debug.Log($"BeginMovement: {actor.player.PlayerName}");
    }

    private void OnEndMovement(TurnActor actor)
    {
        Debug.Log($"EndMovement: {actor.player.PlayerName}");
    }

    private void OnBeginBattle(TurnActor actor)
    {
        Debug.Log($"BeginBattle: {actor.player.PlayerName}");
    }

    private void OnEndBattle(TurnActor actor)
    {
        Debug.Log($"EndBattle: {actor.player.PlayerName}");
    }

    private void OnBeginChallenge(TurnActor actor)
    {
        Debug.Log($"BeginChallenge: {actor.player.PlayerName}");
    }

    private void OnEndChallenge(TurnActor actor)
    {
        Debug.Log($"EndChallenge: {actor.player.PlayerName}");
    }
}
