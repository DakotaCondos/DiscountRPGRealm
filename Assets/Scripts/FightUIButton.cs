using UnityEngine;

public class FightUIButton : MonoBehaviour
{
    public Player player;
    public FightPanelUI fightPanelUI;

    public void SendPlayerToFightPanel()
    {
        fightPanelUI.SelectOpponent(player);
    }
}
