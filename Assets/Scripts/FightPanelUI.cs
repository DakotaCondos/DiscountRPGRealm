using Nova;
using NovaSamples.UIControls;
using System.Collections.Generic;
using UnityEngine;

public class FightPanelUI : MonoBehaviour
{
    public UIBlock2D playerBackingBlock;
    public UIBlock2D playerImageBlock;
    public TextBlock playerPowerTextBlock;

    public UIBlock2D opponentBackingBlock;
    public UIBlock2D opponentImageBlock;
    public TextBlock opponentPowerTextBlock;

    public UIBlock2D buttonsBlock;
    public GameObject buttonPrefab;
    public UIBlock2D fightButton;
    public Texture2D defaultTexture;
    private List<GameObject> buttonInstanceList = new();
    private List<Player> opponents = new();
    private Player player;
    private Player opponent;
    public UIBlock2D backgroundImageBlock;


    private void OnEnable() => backgroundImageBlock.SetImage(TurnManager.Instance.GetCurrentActor().player.currentSpace.image);

    public void CreateButtons(List<Player> players)
    {
        opponents.AddRange(players);
        foreach (Player player in opponents)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsBlock.transform);
            buttonInstanceList.Add(button);
            button.GetComponentInChildren<TextBlock>().Text = player.PlayerName;
            FightUIButton fb = button.AddComponent<FightUIButton>();
            fb.player = player;
            fb.fightPanelUI = this;
            button.GetComponent<Button>().OnClicked.AddListener(fb.SendPlayerToFightPanel);
        }
    }

    public void CreatePlayer(Player player, bool isOpponent = false)
    {
        UIBlock2D targetUIBack = (isOpponent) ? opponentBackingBlock : playerBackingBlock;
        UIBlock2D targetUIImage = (isOpponent) ? opponentImageBlock : playerImageBlock;
        TextBlock targetUITextBlock = (isOpponent) ? opponentPowerTextBlock : playerPowerTextBlock;
        targetUIBack.Color = player.playerColor;
        targetUIImage.SetImage(player.playerTexture);
        targetUITextBlock.Text = player.GetPowerVsPlayer().ToString();
        if (isOpponent)
        {
            this.opponent = player;
        }
        else
        {
            this.player = player;
        }
        EnableFightButton(isOpponent);
    }

    public void ResetPanel()
    {
        playerBackingBlock.Color = Color.black;
        opponentBackingBlock.Color = Color.black;
        playerImageBlock.SetImage(defaultTexture);
        opponentImageBlock.SetImage(defaultTexture);
        playerPowerTextBlock.Text = "";
        opponentPowerTextBlock.Text = "";
        opponents.Clear();
        EnableFightButton(false);

        for (int i = 0; i < buttonInstanceList.Count; i++)
        {
            Destroy(buttonInstanceList[i]);
        }
        buttonInstanceList.Clear();
    }

    public void SelectOpponent(Player player)
    {
        CreatePlayer(player, true);
        opponent = player;
    }

    public void EnableFightButton(bool value)
    {
        fightButton.gameObject.SetActive(value);
    }

    public void Fight()
    {
        TurnState.TriggerBeginBattlePvP(player, opponent);
        ResetPanel();
    }
}
