using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginBattleHandler : MonoBehaviour
{
    private void OnEnable()
    {
        TurnState.BeginBattle += HandleBeginBattle;
    }

    private void OnDisable()
    {
        TurnState.BeginBattle -= HandleBeginBattle;
    }

    private void HandleBeginBattle(TurnActor actor)
    {
        // Handle BeginBattle event here
    }
}
