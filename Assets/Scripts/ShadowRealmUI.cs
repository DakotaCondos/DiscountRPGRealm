using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShadowRealmUI : MonoBehaviour
{
    private float _orthographicSizeOriginal;
    [SerializeField] private float _gameOrthographicSize;
    [SerializeField] private GameObject _plinkoGame;
    [SerializeField] private GameObject _playButton;

    private void OnEnable()
    {
        _playButton.SetActive(true);
        _orthographicSizeOriginal = Camera.main.orthographicSize;
        Camera.main.orthographicSize = _gameOrthographicSize;
        _plinkoGame.SetActive(true);
        SetUIBlockVisuals();
        CameraController.Instance.SetFocusObject(_plinkoGame);
        PlinkoGame.GameResult += ProcessGameResult;
    }

    private void OnDisable()
    {
        Camera.main.orthographicSize = _orthographicSizeOriginal;
        _plinkoGame.SetActive(false);
        CameraController.Instance.ClearFocusObject(TurnManager.Instance.GetCurrentActor().player.currentSpace.gameObject);
        PlinkoGame.GameResult -= ProcessGameResult;
    }

    public void DisableButton()
    {
        _playButton.SetActive(false);
    }

    public async void ProcessGameResult(bool win)
    {
        //Let players process result
        await Task.Delay(3000);

        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.mainPanel);

        if (win)
        {
            GameBoard.Instance.StartingSpace.SelectSpace();
        }
        else
        {
            TurnManager.Instance.NextTurn();
        }
    }

    private void SetUIBlockVisuals()
    {
        Player player = TurnManager.Instance.GetCurrentActor().player;
        PlinkoGame plinkoGame = PlinkoGame.Instance;
        plinkoGame.SetBackgroundImage(player.currentSpace.image);
        plinkoGame.SetBallImage(player.playerTexture);
        plinkoGame.SetBallColor(player.playerColor);
    }

}