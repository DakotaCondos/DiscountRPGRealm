using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameHandler : MonoBehaviour
{
    GameBoard gameBoard;
    TurnManager turnManager;
    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        turnManager = FindObjectOfType<TurnManager>();
    }

    private void OnEnable()
    {
        TurnState.StartGame += HandleStartGame;
    }

    private void OnDisable()
    {
        TurnState.StartGame -= HandleStartGame;
    }

    private void HandleStartGame()
    {
        if (gameBoard == null) { Debug.LogWarning("StartGameHandler could not find GameBoard"); }
        gameBoard.InitializeGameBoard();


        // Trigger BeginTurn
        if (gameBoard == null) { Debug.LogWarning("StartGameHandler could not find TurnManager"); }
        TurnState.TriggerBeginTurn(turnManager.GetCurrentPlayer());
    }
}
