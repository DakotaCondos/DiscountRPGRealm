using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeleportGroup
{
    public Space TeleportStartSpace;
    public Space TeleportEndSpace;
    public Space EnableTeleportGroupSpace;
}

public class TeleportManager : MonoBehaviour
{
    [Header("Teleport Groups")]
    [SerializeField] private List<TeleportGroup> _teleportGroups = new();


    public void EvaluateSpaces(Player player)
    {
        Space currentPlayerSpace = player.currentSpace;

        foreach (TeleportGroup teleportGroup in _teleportGroups)
        {
            // If on an enabling space and not already connected
            if (teleportGroup.EnableTeleportGroupSpace.Equals(currentPlayerSpace)
                 && !teleportGroup.TeleportStartSpace.ConnectedSpaces.Contains(teleportGroup.TeleportEndSpace))
            {
                GameBoard.Instance.ConnectSpaces(teleportGroup.TeleportStartSpace, teleportGroup.TeleportEndSpace);
                // Play some effect here
            }
        }
    }
}
