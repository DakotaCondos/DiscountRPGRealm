using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnHandler : MonoBehaviour
{
    TurnOrderPanel turnOrderPanel;

    private void Awake()
    {
        turnOrderPanel = FindObjectOfType<TurnOrderPanel>();
    }

    private void OnEnable()
    {
        TurnState.EndTurn += HandleEndTurn;
    }

    private void OnDisable()
    {
        TurnState.EndTurn -= HandleEndTurn;
    }

    public void HandleEndTurn(TurnActor actor)
    {
        // Handle EndTurn event here
        turnOrderPanel.UpdatePanel();
    }
}
