using UnityEngine;

public class DevTestKit : MonoBehaviour
{
    public Player player1;
    public Texture2D player1Texture;

    public Player player2;
    public Texture2D player2Texture;

    public Monster monster;
    public MonsterSO monsterSO;


    private void Start()
    {
        player1 = new("TestPlayer1", 0, Color.gray, player1Texture);
        player2 = new("TestPlayer2", 0, Color.gray, player2Texture);
        monster = new(monsterSO);
    }

    [ContextMenu("PVP Win")]
    public void PVPWin()
    {
        BattleUI battleUI = FindObjectOfType<BattleUI>();
        if (battleUI != null)
        {
            battleUI.BuildBattleUI(player1, player2, true);
        }
    }

    [ContextMenu("PVP Lose")]
    public void PVPLose()
    {
        BattleUI battleUI = FindObjectOfType<BattleUI>();
        if (battleUI != null)
        {
            battleUI.BuildBattleUI(player1, player2, false);
        }
    }

    [ContextMenu("PVM Win")]
    public void PVMWin()
    {
        BattleUI battleUI = FindObjectOfType<BattleUI>();
        if (battleUI != null)
        {
            battleUI.BuildBattleUI(player1, monster, true);
        }
    }

    [ContextMenu("PVM Lose")]
    public void PVMLose()
    {
        BattleUI battleUI = FindObjectOfType<BattleUI>();
        if (battleUI != null)
        {
            battleUI.BuildBattleUI(player1, monster, false);
        }
    }
}
