using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    // A list to hold the players
    public List<TurnActor> players;
    // A queue to hold the turn order
    public Queue<TurnActor> turnOrder;
    public static TurnManager Instance { get; private set; }

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
        // Initialize the player list and the turn order
        players = new List<TurnActor>();
        foreach (var item in GameManager.Instance.Players)
        {
            players.Add(new TurnActor(item));
        }

        turnOrder = new Queue<TurnActor>();

        // Shuffle players
        ShufflePlayers();

        // Add Creatures TurnActor to end
        players.Add(new TurnActor(new Player("Monsters", 7, Color.red, UIReferences.Instance.monsterIconImage), false));

        // Fill the turn order queue
        foreach (TurnActor player in players)
        {
            turnOrder.Enqueue(player);
        }
    }

    // Method to shuffle players using Fisher-Yates algorithm
    private void ShufflePlayers()
    {
        for (int i = players.Count - 1; i > 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i + 1);
            (players[rand], players[i]) = (players[i], players[rand]);
        }
    }

    // Method to return the current player's turn
    public TurnActor GetCurrentActor()
    {
        if (turnOrder.Count > 0)
        {
            return turnOrder.Peek();
        }
        return null;
    }

    // Method to advance to the next player's turn
    [ContextMenu("Next Turn")]
    public void NextTurn()
    {
        if (turnOrder.Count > 0)
        {
            // Move the current player to the back of the queue
            TurnActor currentPlayer = turnOrder.Dequeue();
            turnOrder.Enqueue(currentPlayer);

            // Invoke the EndTurn event with the former player
            TurnState.TriggerEndTurn(currentPlayer);

            // Invoke the EndTurn event with the current player
            TurnState.TriggerBeginTurn(GetCurrentActor());
        }
    }

    // Method to get the order of the following players awaiting their turn
    public List<TurnActor> GetUpcomingPlayers()
    {
        List<TurnActor> upcomingTurns = new(turnOrder.ToArray());
        // Remove the first element (current player)
        upcomingTurns.RemoveAt(0);
        return upcomingTurns;
    }
}
