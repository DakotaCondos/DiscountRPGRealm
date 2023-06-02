using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpaceBuilder))]
public class GameBoard : MonoBehaviour
{
    public List<Space> spaces = new List<Space>();
    private SpaceBuilder spaceBuilder;
    [SerializeField] float spaceConnectionRadius;

    public GameBoard()
    {
        spaces = new List<Space>();
    }

    public void Awake()
    {
        // May manually assign spaces to groups later for different Zones related to game areas
        spaces = FindObjectsOfType<Space>(true).ToList();
        print(spaces.Count);

        spaceBuilder = GetComponent<SpaceBuilder>();
    }

    public void InitializeGameBoard()
    {
        print("Building Spaces");
        foreach (Space space in spaces)
        {
            // build space
            spaceBuilder.BuildSpace(space);
        }

        print("Building Connections");
        // Build Connections
        ConnectNearbySpaces(spaces, spaceConnectionRadius);

        print("Initializing Spaces");
        foreach (Space space in spaces)
        {
            // initialize space
            space.Initialize();
        }
    }

    public void ConnectSpaces(Space a, Space b)
    {
        a.ConnectedSpaces.Add(b);
        b.ConnectedSpaces.Add(a);
    }

    public void ConnectNearbySpaces(List<Space> spaces, float maxDistance)
    {
        for (int i = 0; i < spaces.Count; i++)
        {
            for (int j = i + 1; j < spaces.Count; j++)
            {
                // debug step display distances
                float distance = Vector3.Distance(spaces[i].transform.position, spaces[j].transform.position);
                print($"{spaces[i].namePlate.Text} -> {spaces[j].namePlate.Text} = {distance}");

                if (Vector3.Distance(spaces[i].transform.position, spaces[j].transform.position) <= maxDistance)
                {
                    ConnectSpaces(spaces[i], spaces[j]);
                }
            }
        }
    }

    public Dictionary<Space, int> CalculateDistances(Space start)
    {
        Queue<Space> queue = new Queue<Space>();
        Dictionary<Space, bool> visited = new Dictionary<Space, bool>();
        Dictionary<Space, int> distance = new Dictionary<Space, int>();

        foreach (var space in spaces)
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

        foreach (var space in spaces)
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
