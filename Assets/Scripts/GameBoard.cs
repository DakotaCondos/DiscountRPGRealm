using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(SpaceBuilder))]
[RequireComponent(typeof(LineDrawer))]
public class GameBoard : MonoBehaviour
{
    public List<Space> allSpaces = new List<Space>();
    private SpaceBuilder spaceBuilder;
    private LineDrawer lineDrawer;
    [SerializeField] float spaceConnectionRadius;

    public GameBoard()
    {
        allSpaces = new List<Space>();
    }

    public void Awake()
    {
        spaceBuilder = GetComponent<SpaceBuilder>();
        lineDrawer = GetComponent<LineDrawer>();

        // May manually assign spaces to groups later for different Zones related to game areas
        allSpaces = FindObjectsOfType<Space>(true).ToList();
    }

    public void InitializeGameBoard()
    {
        foreach (Space space in allSpaces)
        {
            // build space
            spaceBuilder.BuildSpace(space);

            // initialize space
            space.Initialize();

        }
        // Build Connections
        ConnectNearbySpaces(allSpaces, spaceConnectionRadius);
    }

    public void ConnectSpaces(Space a, Space b)
    {
        a.ConnectedSpaces.Add(b);
        b.ConnectedSpaces.Add(a);
    }

    // debug step display distances
    //float distance = Vector3.Distance(spaces[i].transform.position, spaces[j].transform.position);

    public void ConnectNearbySpaces(List<Space> allSpaces, float maxDistance)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Space currentSpace in allSpaces)
        {
            foreach (Space space in allSpaces)
            {
                if (currentSpace.Equals(space)) { continue; }
                if (currentSpace.ConnectedSpaces.Contains(space)) { continue; }
                stringBuilder.AppendLine($"{currentSpace.namePlate.Text} -> {space.namePlate.Text} = {Vector3.Distance(currentSpace.lineDrawPoint.transform.position, space.lineDrawPoint.transform.position)}");
                if (Vector3.Distance(currentSpace.lineDrawPoint.transform.position, space.lineDrawPoint.transform.position) <= spaceConnectionRadius)
                {
                    ConnectSpaces(currentSpace, space);
                    lineDrawer.DrawLine(currentSpace.lineDrawPoint, space.lineDrawPoint);
                }
            }
        }
        print(stringBuilder);
    }

    public Dictionary<Space, int> CalculateDistances(Space start)
    {
        Queue<Space> queue = new Queue<Space>();
        Dictionary<Space, bool> visited = new Dictionary<Space, bool>();
        Dictionary<Space, int> distance = new Dictionary<Space, int>();

        foreach (var space in allSpaces)
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

        foreach (var space in allSpaces)
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
