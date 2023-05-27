using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMovementHandler : MonoBehaviour
{
    private void OnEnable()
    {
        TurnState.EndMovement += HandleEndMovement;
    }

    private void OnDisable()
    {
        TurnState.EndMovement -= HandleEndMovement;
    }

    private void HandleEndMovement(TurnActor actor)
    {
        // Handle EndMovement event here
    }
}
