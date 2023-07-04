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
    public static GameBoard Instance { get; private set; }

    public List<Space> allSpaces = new List<Space>();
    private SpaceBuilder spaceBuilder;
    private LineDrawer lineDrawer;
    [SerializeField] float spaceConnectionRadius;
    public ActorPieceMovement actorPieceMovement;
    public Space StartingSpace;
    public Space ShadowRealmCitySpace;

    public void Awake()
    {
        // Instance setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        spaceBuilder = GetComponent<SpaceBuilder>();
        lineDrawer = GetComponent<LineDrawer>();

        // May manually assign spaces to groups later for different Zones related to game areas
        allSpaces = FindObjectsOfType<Space>(true).ToList();
        actorPieceMovement = FindObjectOfType<ActorPieceMovement>(true);
    }

    public void InitializeGameBoard()
    {
        foreach (Space space in allSpaces)
        {
            // build space
            spaceBuilder.BuildSpace(space);

            // initialize space
            space.Initialize();
            if (space.isStartingSpace) { StartingSpace = space; }
        }
        // Build Connections
        ConnectNearbySpaces(allSpaces, spaceConnectionRadius);
    }

    public void ConnectSpaces(Space a, Space b)
    {
        a.ConnectedSpaces.Add(b);
        b.ConnectedSpaces.Add(a);
        lineDrawer.DrawLine(a.lineDrawPoint, b.lineDrawPoint);
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
                if (currentSpace.Equals(space) || currentSpace.customConnections || space.customConnections) { continue; }
                if (currentSpace.ConnectedSpaces.Contains(space)) { continue; }
                stringBuilder.AppendLine($"{currentSpace.namePlate.Text} -> {space.namePlate.Text} = {Vector3.Distance(currentSpace.lineDrawPoint.transform.position, space.lineDrawPoint.transform.position)}");
                if (Vector3.Distance(currentSpace.lineDrawPoint.transform.position, space.lineDrawPoint.transform.position) <= spaceConnectionRadius)
                {
                    ConnectSpaces(currentSpace, space);
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

    public List<Space> FindPath(Space startSpace, Space endSpace)
    {
        // Give a direct path when teleporting
        if (startSpace.spaceType == SpaceType.Transport) { return new List<Space> { startSpace, endSpace }; }

        // Use a dictionary to keep track of visited spaces and their previous spaces
        Dictionary<Space, Space> visitedSpaces = new Dictionary<Space, Space>();
        Queue<Space> queue = new Queue<Space>();

        // Start by visiting the start space
        visitedSpaces[startSpace] = null;
        queue.Enqueue(startSpace);

        while (queue.Count > 0)
        {
            Space currentSpace = queue.Dequeue();

            // If we've reached the end space, we can construct the path and return it
            if (currentSpace == endSpace)
            {
                List<Space> path = new List<Space>();
                while (currentSpace != null)
                {
                    path.Insert(0, currentSpace);
                    currentSpace = visitedSpaces[currentSpace];
                }
                return path;
            }

            // Otherwise, add all unvisited connected spaces to the queue. If it is the end space, add it even if it is blocked
            foreach (Space connectedSpace in currentSpace.ConnectedSpaces)
            {
                if ((!connectedSpace.IsBlocking || connectedSpace == endSpace) && !visitedSpaces.ContainsKey(connectedSpace))
                {
                    visitedSpaces[connectedSpace] = currentSpace;
                    queue.Enqueue(connectedSpace);
                }
            }
        }

        // If we haven't returned by now, no path exists
        Debug.Log("No Valid Path Found");
        return new List<Space> { startSpace, endSpace };
    }
}
