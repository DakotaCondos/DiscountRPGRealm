using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public bool Fight(Player player, Player opponent)
    {
        float winPercentage = (float)player.GetPowerVsPlayer() / (float)(player.GetPowerVsPlayer() + opponent.GetPowerVsPlayer());
        bool win = winPercentage >= Random.value;
        return win;
    }

    public bool Fight(Player player, Monster opponent)
    {
        float winPercentage = (float)player.GetPowerVsMonster() / (float)(player.GetPowerVsMonster() + opponent.power);
        bool win = winPercentage >= Random.value;
        return win;
    }

    public async void CreateEncounter(Player player, Player opponent)
    {
        // Results (may change this later)
        bool win = Fight(player, opponent);

        // Setup fight scene visuals here
        await Task.Delay(100);// Display battle stuff, dummy task for now

        // Handle Results
        if (win)
        {
            ConsolePrinter.PrintToConsole($"{player.PlayerName} Wins!", Color.green);
        }
        else
        {
            ConsolePrinter.PrintToConsole($"{player.PlayerName} Lost!", Color.red);
        }
        TurnState.TriggerEndBattlePvP(player, opponent, win);
    }

    public async void CreateEncounter(Player player, Monster opponent)
    {
        // Results (may change this later)
        bool win = Fight(player, opponent);

        // Setup fight scene visuals here
        await Task.Delay(100);// Display battle stuff, dummy task for now

        // Handle Results
        if (win)
        {
            ConsolePrinter.PrintToConsole($"{player.PlayerName} Wins!", Color.green);
        }
        else
        {
            ConsolePrinter.PrintToConsole($"{player.PlayerName} Lost!", Color.red);
            ScaleMonsterPower(opponent);
        }
        TurnState.TriggerEndBattlePvM(player, opponent, win);
    }

    public void ScaleMonsterPower(Monster monster)
    {
        // Special monsters first
        if (monster.MonsterName == "Slime")
        {
            monster.power += 1;
            monster.power *= 2;
            if (monster.power > 100) { monster.power = 100; }
            UpdateMonsterPiece(monster);
            return;
        }

        switch (monster.power)
        {
            case <= 10:
                monster.power += 2;
                break;
            case > 10 and <= 25:
                monster.power += 3;
                break;
            case > 25 and <= 50:
                monster.power += 5;
                break;
            case > 50 and <= 75:
                monster.power += 8;
                break;
            case > 75 and <= 90:
                monster.power += 10;
                break;
            default:
                monster.power = (int)(1.025 * monster.power);
                break;
        }

        UpdateMonsterPiece(monster);
    }

    public void UpdateMonsterPiece(Monster monster)
    {
        monster.currentSpace.monsterPieceAtSpace.GetComponent<MonsterPiece>().powerLabel.Text = monster.power.ToString();
    }
}
