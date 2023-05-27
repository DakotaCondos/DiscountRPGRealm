using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginChallengeHandler : MonoBehaviour
{
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
        // Handle BeginChallenge event here
    }
}
