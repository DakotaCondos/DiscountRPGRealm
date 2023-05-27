using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattleHandler : MonoBehaviour
{
    private void OnEnable()
    {
        TurnState.EndBattle += HandleEndBattle;
    }

    private void OnDisable()
    {
        TurnState.EndBattle -= HandleEndBattle;
    }

    private void HandleEndBattle(TurnActor actor)
    {
        // Handle EndBattle event here
    }
}
