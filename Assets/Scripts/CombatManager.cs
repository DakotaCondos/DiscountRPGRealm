using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public BattleUI battleUI;

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

        battleUI = FindObjectOfType<BattleUI>(true);
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

    public void CreateEncounter(Player player, Player opponent)
    {
        // Results
        bool win = Fight(player, opponent);

        if (win)
        {
            player.PVPwins++;
            opponent.PVPlosses++;
        }
        else
        {
            opponent.PVPwins++;
            player.PVPlosses++;
        }

        BattleDTO battleDTO = new()
        {
            player = player,
            playerOpponent = opponent,
            monster = null,
            win = win,
        };

        // Setup fight scene visuals here
        battleUI.InitiateBattleSequence(battleDTO);
    }

    public void CreateEncounter(Player player, Monster opponent)
    {
        // Results
        bool win = Fight(player, opponent);

        if (win)
        {
            player.PVMwins++;
        }
        else
        {
            player.PVMlosses++;
        }

        BattleDTO battleDTO = new()
        {
            player = player,
            playerOpponent = null,
            monster = opponent,
            win = win,
        };

        // Setup fight scene visuals here
        battleUI.InitiateBattleSequence(battleDTO);
    }

    public void BattleDTOHandler(BattleDTO battleDTO)
    {
        if (battleDTO.playerOpponent != null)
        {
            print("Processing PVP"); //debug delete
            TurnState.TriggerEndBattlePvP(battleDTO.player, battleDTO.playerOpponent, battleDTO.win);
        }
        else
        {
            print("Processing PVM"); //debug delete
            TurnState.TriggerEndBattlePvM(battleDTO.player, battleDTO.monster, battleDTO.win);
        }
    }
}
