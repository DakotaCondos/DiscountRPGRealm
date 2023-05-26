using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderPanel : MonoBehaviour
{
    public NameBlock activePlayer;
    public List<NameBlock> upcomingPlayerBlocks;
    public GameObject playerBlockPrefab;
    public Transform playerBlockLocation;
    [SerializeField] TurnManager turnManager;
    public Color[] colorIDs;

    [SerializeField] int defalutActivePlayerTextSize;
    [SerializeField] int defalutUpcomingTextSize;

    [ContextMenu("Update Panel")]

    private void Start()
    {
        foreach (var item in turnManager.GetUpcomingPlayers())
        {
            GameObject g = Instantiate(playerBlockPrefab, playerBlockLocation);
            upcomingPlayerBlocks.Add(g.GetComponent<NameBlock>());
        }

        UpdatePanel();
    }

    public void UpdatePanel()
    {
        //update Active Player Block
        UpdateBlock(activePlayer, turnManager.GetCurrentPlayer().player);
        ResizeTextToFitContainer(activePlayer);

        List<TurnActor> upcomingPlayers = turnManager.GetUpcomingPlayers();
        for (int i = 0; i < upcomingPlayers.Count; i++)
        {
            UpdateBlock(upcomingPlayerBlocks[i], upcomingPlayers[i].player);
            ResizeTextToFitContainer(upcomingPlayerBlocks[i]);
        }
    }

    private void UpdateBlock(NameBlock nameBlock, Player player)
    {
        nameBlock.SetGradient(colorIDs[player.TeamID]);
        nameBlock.SetLabel(player.PlayerName);
        if (nameBlock.Equals(activePlayer))
        {
            nameBlock.labelBlock.TMP.fontSize = defalutActivePlayerTextSize;
        }
        else
        {
            nameBlock.labelBlock.TMP.fontSize = defalutUpcomingTextSize;
        }
    }

    private void ResizeTextToFitContainer(NameBlock nameBlock)
    {
        float containerSizeX = nameBlock.containerBlock.Size.X.Raw;
        float labelSizeX = nameBlock.labelBlock.Size.X.Raw;

        if (labelSizeX > containerSizeX)
        {
            nameBlock.labelBlock.TMP.fontSize -= 5;
            nameBlock.labelBlock.CalculateLayout();
            ResizeTextToFitContainer(nameBlock);
        }
    }
}
