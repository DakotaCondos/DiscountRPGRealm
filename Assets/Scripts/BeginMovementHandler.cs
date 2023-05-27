using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginMovementHandler : MonoBehaviour
{
    private void OnEnable()
    {
        TurnState.BeginMovement += HandleBeginMovement;
    }

    private void OnDisable()
    {
        TurnState.BeginMovement -= HandleBeginMovement;
    }

    private void HandleBeginMovement(TurnActor actor)
    {
        // Handle BeginMovement event here
    }
}
