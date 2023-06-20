using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int createdMonsters = 0;
    public int defeatedMonsters = 0;
    public List<Space> spawnableSpaces = new List<Space>(); // List of spawn spaces
    public List<Monster> monsters = new List<Monster>(); // List of spawned monsters
    public List<MonsterSO> monsterSOs = new List<MonsterSO>(); // List of spawned monsters
    public MonsterSO defaultMonster;
    public float spawnChance = 0.25f; // Percentage chance to spawn a monster
    public float moveChance = 0.25f; // Percentage chance to move a monster

    public static MonsterManager Instance { get; private set; }

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

    private void Start()
    {
        spawnableSpaces = GameBoard.Instance.allSpaces.Where(space => space.canSpawnMonsters).ToList();
    }

    public async Task ProcessMonsterTurn()
    {
        // Move monsters
        foreach (Monster monster in monsters)
        {
            List<Space> availableSpaces = monster.currentSpace.ConnectedSpaces
                .Where(space => space.canMonstersTraverse && !space.hasMonster)
                .ToList();

            if (availableSpaces.Count > 0 && moveChance >= Random.value)
            {
                monster.currentSpace.RemoveMonsterFromSpace();
                Space randomSpace = availableSpaces[Random.Range(0, availableSpaces.Count)];
                // do async game board movement stuff
                await PerformGameBoardMovementAsync(monster, randomSpace);
                randomSpace.AddMonsterToSpace(monster);
                await Task.Delay(500); // Let your brain process what just happened
            }
        }

        // Spawn monsters
        foreach (Space space in spawnableSpaces)
        {
            if (monsters.Count >= 2 * spawnableSpaces.Count) { continue; }

            if (space.hasMonster) { continue; }

            if (spawnChance >= Random.value)
            {
                await SpawnMonster(space);
                await Task.Delay(500); // Let your brain process what just happened
            }
        }
    }

    private MonsterSO GetMonsterSO(int maxPower)
    {
        List<MonsterSO> eligibleMonsters = monsterSOs.Where(monster => monster.power <= maxPower && monster.doesMove).ToList();

        if (eligibleMonsters.Count > 0)
        {
            int randomIndex = Random.Range(0, eligibleMonsters.Count);
            return eligibleMonsters[randomIndex];
        }
        else
        {
            return defaultMonster;
        }
    }

    private async Task PerformGameBoardMovementAsync(Monster monster, Space endSpace)
    {
        TaskHelper helper = new();
        GameBoard.Instance.actorPieceMovement.MoveMonster(monster, monster.currentSpace, endSpace, helper);
        while (!helper.isComplete)
        {
            await Task.Delay(100); // Wait for 100 milliseconds before checking again
        }
    }

    private async Task SpawnMonster(Space space)
    {
        int avgPower = 0;
        int lowestPower = int.MaxValue;
        foreach (Player player in GameManager.Instance.Players)
        {
            avgPower += player.GetPowerVsMonster();
            if (lowestPower > player.GetPowerVsMonster()) { lowestPower = player.GetPowerVsMonster(); }
        }
        avgPower /= GameManager.Instance.Players.Count;
        print(avgPower);

        MonsterSO blueprint = GetMonsterSO(avgPower);
        Monster monster = new Monster(blueprint);

        TaskHelper helper = new();
        await MonsterPieceSpawner.Instance.SpawnMonster(monster, space, helper);
        while (!helper.isComplete)
        {
            await Task.Delay(100); // Wait for 100 milliseconds before checking again
        }

        monsters.Add(monster);
        space.AddMonsterToSpace(monster);
        createdMonsters++;
    }

    public void KillMonster(Monster monster, Player player)
    {
        int xpToAdd = monster.power / 15;
        if (xpToAdd <= 0) { xpToAdd = 1; }
        int moneyDropped = 3 + (monster.power / 4);
        player.effects.Enqueue(new(PlayerEffectType.XP, xpToAdd));
        player.effects.Enqueue(new(PlayerEffectType.Money, moneyDropped));
        monsters.Remove(monster);
        monster.currentSpace.RemoveMonsterFromSpace();
        defeatedMonsters++;
    }

    public void ScaleMonsterPower(Monster monster)
    {
        // Special monsters first
        if (monster.MonsterName == "Slime")
        {
            int startingPower = monster.power;
            monster.power *= 2;
            monster.power += 1;
            if (monster.power > 100) { monster.power = 100; }
            if (monster.power - startingPower == 0) { return; } // skip 0 value changes
            monster.effects.Enqueue(new(PlayerEffectType.Power, monster.power - startingPower));
            UpdateMonsterPiece(monster);

            return;
        }


        int powerUp = monster.power switch
        {
            <= 10 => 2,
            > 10 and <= 25 => 3,
            > 25 and <= 50 => 5,
            > 50 and <= 75 => 8,
            > 75 and <= 90 => 10,
            _ => (int)(1.025 * monster.power),
        };
        monster.power += powerUp;
        monster.effects.Enqueue(new(PlayerEffectType.Power, powerUp));
        UpdateMonsterPiece(monster);
    }

    public void UpdateMonsterPiece(Monster monster)
    {
        monster.currentSpace.monsterPieceAtSpace.GetComponent<MonsterPiece>().powerLabel.Text = monster.power.ToString();
    }
}
