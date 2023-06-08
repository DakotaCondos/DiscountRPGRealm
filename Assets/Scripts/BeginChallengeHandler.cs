using UnityEngine;

public class BeginChallengeHandler : MonoBehaviour
{
    GuessingGame guessingGame;
    private void Awake()
    {
        guessingGame = FindObjectOfType<GuessingGame>(true);

    }
    private void OnEnable()
    {
        TurnState.BeginChallenge += HandleBeginChallenge;
    }

    private void OnDisable()
    {
        TurnState.BeginChallenge -= HandleBeginChallenge;
    }

    private void HandleBeginChallenge(TurnActor actor)
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled) { ConsolePrinter.PrintToConsole($"HandleBeginChallenge({actor.player.PlayerName})", Color.cyan); }
        // Handle BeginChallenge event here
        int difficulty = Random.Range(1, 11);
        CameraController.Instance.snapToOutOfBoundsView = true;
        guessingGame.SetupGame(difficulty);
    }
}
