using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public List<Space> Spaces { get; set; }
    private bool isInitialized = false;

    public GameBoard()
    {
        Spaces = new List<Space>();
    }

    public void Awake()
    {
        Spaces = FindObjectsOfType<Space>(true).ToList();
    }

    private void Update()
    {
        if (!isInitialized)
        {
            InitializeGameBoard();
        }
    }

    private void InitializeGameBoard()
    {
        foreach (var space in Spaces)
        {
            space.Initialize();
        }


        isInitialized = true;
    }

    public void ConnectSpaces(Space a, Space b)
    {
        a.ConnectedSpaces.Add(b);
        b.ConnectedSpaces.Add(a);
    }

    public Dictionary<Space, int> CalculateDistances(Space start)
    {
        Queue<Space> queue = new Queue<Space>();
        Dictionary<Space, bool> visited = new Dictionary<Space, bool>();
        Dictionary<Space, int> distance = new Dictionary<Space, int>();

        foreach (var space in Spaces)
        {
            visited[space] = false;
            distance[space] = int.MaxValue;
        }

        visited[start] = true;
        distance[start] = 0;
        queue.Enqueue(start);

        while (queue.Count != 0)
        {
            Space x = queue.Dequeue();

            if (x.IsBlocking) continue;

            foreach (var neighbor in x.ConnectedSpaces)
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    distance[neighbor] = distance[x] + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return distance;
    }
}
