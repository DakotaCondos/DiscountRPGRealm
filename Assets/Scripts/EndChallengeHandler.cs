using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndChallengeHandler : MonoBehaviour
{
    private void OnEnable()
    {
        TurnState.EndChallenge += HandleEndChallenge;
    }

    private void OnDisable()
    {
        TurnState.EndChallenge -= HandleEndChallenge;
    }

    private void HandleEndChallenge(TurnActor actor)
    {
        // Handle EndChallenge event here
    }
}
