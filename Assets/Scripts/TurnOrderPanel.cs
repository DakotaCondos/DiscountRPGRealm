using Nova;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnOrderPanel : MonoBehaviour
{
    private TurnManager turnManager;

    [Header("Active Player")]
    [SerializeField] private UIBlock2D _nameBlock;
    [SerializeField] private UIBlock2D _image;
    [SerializeField] private TextBlock _textBlock;

    [Header("Upcoming Players")]
    [SerializeField] private GameObject _upcomingPlayerRowPrefab;
    private List<UIHelper> _upcomingPlayerUIHelpers = new();
    public Transform playerBlockLocation;
    private void Awake()
    {
        turnManager = TurnManager.Instance;
    }
    private void Start()
    {
        int numUpcomingPlayers = turnManager.GetUpcomingPlayers().Count;
        for (int i = 0; i < numUpcomingPlayers; i++)
        {
            GameObject g = Instantiate(_upcomingPlayerRowPrefab, playerBlockLocation);
            UIHelper j = g.GetComponent<UIHelper>();
            _upcomingPlayerUIHelpers.Add(j);
        }

        UpdatePanel();
    }

    public void UpdatePanel()
    {
        // Update CurrentPlayer
        TurnActor turnActor = turnManager.GetCurrentActor();
        Player activePlayer = turnActor.player;
        _textBlock.Text = activePlayer.PlayerName;
        _nameBlock.Border.Color = activePlayer.playerColor;
        _nameBlock.Gradient.Color = activePlayer.playerColor;
        _image.Border.Color = activePlayer.playerColor;

        _image.SetImage(activePlayer.playerTexture);


        // Update Upcoming Players
        List<TurnActor> upcomingPlayers = turnManager.GetUpcomingPlayers();
        for (int i = 0; i < upcomingPlayers.Count; i++)
        {
            UIHelperUpdate(_upcomingPlayerUIHelpers[i], upcomingPlayers[i].player);
        }
    }

    private void UIHelperUpdate(UIHelper uiHelper, Player player)
    {
        Color c = player.playerColor;
        uiHelper.TextBlocks[0].Text = player.PlayerName;
        uiHelper.UIBlock2Ds[0].Border.Color = c;
        uiHelper.UIBlock2Ds[0].Gradient.Color = c;
        uiHelper.UIBlock2Ds[1].Border.Color = c;
        uiHelper.UIBlock2Ds[1].SetImage(player.playerTexture);
    }
}
