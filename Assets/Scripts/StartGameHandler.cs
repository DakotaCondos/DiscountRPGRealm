using System;
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

        //Difficulty Modifications
        GameDifficulty gameDifficulty = GameManager.Instance.gameDifficulty;
        switch (gameDifficulty)
        {
            case GameDifficulty.Relaxed:
                PlayerPowerAdjust(3);
                PlayerMoneyAdjust(5);
                break;
            case GameDifficulty.Easy:
                PlayerPowerAdjust(1);
                PlayerMoneyAdjust(2);
                break;
            case GameDifficulty.Normal:
                break;
            case GameDifficulty.Hard:
                break;
            case GameDifficulty.Insaine:
                break;
            default:
                break;
        }
        // Trigger BeginTurn
        if (gameBoard == null) { Debug.LogWarning("StartGameHandler could not find TurnManager"); }
        TurnState.TriggerBeginTurn(turnManager.GetCurrentActor());
    }

    private void PlayerPowerAdjust(int v)
    {
        foreach (Player player in GameManager.Instance.Players)
        {
            if (player.TeamID == 7) { continue; }
            player.AddPower(v);
        }
    }

    private void PlayerMoneyAdjust(int v)
    {
        foreach (Player player in GameManager.Instance.Players)
        {
            if (player.TeamID == 7) { continue; }
            player.AddMoney(v);
        }
    }
}
