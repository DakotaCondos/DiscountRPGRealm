using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpaceBuilder))]
public class GameBoard : MonoBehaviour
{
    public List<Space> Spaces { get; set; }
    private SpaceBuilder SpaceBuilder;

    public GameBoard()
    {
        Spaces = new List<Space>();
    }

    public void Awake()
    {
        Spaces = FindObjectsOfType<Space>(true).ToList();
        SpaceBuilder = GetComponent<SpaceBuilder>();
    }

    public void InitializeGameBoard()
    {
        foreach (var space in Spaces)
        {
            // build space

            // initialize space
            space.Initialize();
        }
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

    public List<Space> SpacesInRange(Space start, int maxDistance)
    {
        Queue<Space> queue = new Queue<Space>();
        Dictionary<Space, bool> visited = new Dictionary<Space, bool>();
        Dictionary<Space, int> distance = new Dictionary<Space, int>();
        List<Space> spacesWithinDistance = new List<Space>();

        foreach (var space in Spaces)
        {
            visited[space] = false;
        }

        visited[start] = true;
        distance[start] = 0;
        queue.Enqueue(start);
        spacesWithinDistance.Add(start);

        while (queue.Count != 0)
        {
            Space x = queue.Dequeue();

            if (x.IsBlocking) continue;

            foreach (var neighbor in x.ConnectedSpaces)
            {
                if (!visited[neighbor])
                {
                    int tentativeDistance = distance[x] + 1;
                    if (tentativeDistance <= maxDistance)
                    {
                        visited[neighbor] = true;
                        distance[neighbor] = tentativeDistance;
                        queue.Enqueue(neighbor);
                        spacesWithinDistance.Add(neighbor);
                    }
                }
            }
        }

        return spacesWithinDistance;
    }
}
