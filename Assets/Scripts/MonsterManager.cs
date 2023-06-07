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
                await Task.Delay(100);
            }
        }

        // Spawn monsters
        foreach (Space space in spawnableSpaces)
        {
            if (monsters.Count >= 2 * spawnableSpaces.Count) { continue; }

            if (space.hasMonster) { continue; }

            if (spawnChance >= Random.value) { SpawnMonster(space); }
        }
    }

    private void SpawnMonster(Space space)
    {
        int maxPower = (int)((createdMonsters + defeatedMonsters) * 0.5f * GameManager.Instance.Players.Count);
        MonsterSO blueprint = GetMonsterSO(maxPower);
        Monster monster = new Monster(blueprint);
        monsters.Add(monster);
        space.AddMonsterToSpace(monster);
        createdMonsters++;
    }

    private MonsterSO GetMonsterSO(int maxPower)
    {
        List<MonsterSO> eligibleMonsters = monsterSOs.Where(monster => monster.power <= maxPower).ToList();

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
        TaskHelper helper = new TaskHelper();

        // Perform your game board movement logic asynchronously here
        //print($"Moving {monster.MonsterName} from {monster.currentSpace.namePlate.Text} to {endSpace.namePlate.Text}");
        GameBoard.Instance.actorPieceMovement.MoveMonster(monster, monster.currentSpace, endSpace, helper);

        while (!helper.isComplete)
        {
            await Task.Delay(100); // Wait for 100 milliseconds before checking again
        }
    }

    public void KillMonster(Monster monster, Player player)
    {
        int xpToAdd = monster.power / 20;
        if (xpToAdd <= 0) { xpToAdd = 1; }
        int moneyDropped = 4 + (monster.power / 20);
        player.AddXP(xpToAdd);
        player.AddMoney(moneyDropped);
        monsters.Remove(monster);
        monster.currentSpace.RemoveMonsterFromSpace();
        defeatedMonsters++;
    }
}