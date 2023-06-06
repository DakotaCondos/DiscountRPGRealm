using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMonsterPanelUI : MonoBehaviour
{
    public UIBlock2D playerBackingBlock;
    public UIBlock2D playerImageBlock;
    public TextBlock playerPowerTextBlock;

    public UIBlock2D opponentBackingBlock;
    public UIBlock2D opponentImageBlock;
    public TextBlock monsterPowerTextBlock;

    public TextBlock title;
    public Texture2D defaultTexture;

    Player player;
    Monster monster;

    public void CreatePvM(Player player, Monster monster)
    {
        playerBackingBlock.Color = player.playerColor;
        playerImageBlock.SetImage(player.playerTexture);
        playerPowerTextBlock.Text = player.GetPowerVsMonster().ToString();
        opponentBackingBlock.Color = Color.red;
        opponentImageBlock.SetImage(monster.monsterTexture);
        monsterPowerTextBlock.Text = monster.power.ToString();
        title.Text = $"{player.PlayerName} vs. {monster.MonsterName}";
        this.player = player;
        this.monster = monster;
    }

    public void FightButton()
    {
        TurnState.TriggerBeginBattlePvM(player, monster);
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.mainPanel);
        ResetPanel();
    }

    public void ResetPanel()
    {
        playerBackingBlock.Color = Color.black;
        opponentBackingBlock.Color = Color.black;
        playerImageBlock.SetImage(defaultTexture);
        opponentImageBlock.SetImage(defaultTexture);
        playerPowerTextBlock.Text = "";
        monsterPowerTextBlock.Text = "";
        player = null;
        monster = null;
    }
}
