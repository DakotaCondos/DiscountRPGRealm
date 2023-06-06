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
        }
        TurnState.TriggerEndBattlePvM(player, opponent, win);
    }


}
