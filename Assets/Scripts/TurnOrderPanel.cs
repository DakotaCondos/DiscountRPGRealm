using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnOrderPanel : MonoBehaviour
{
    public NameBlock activePlayer;
    public List<NameBlock> upcomingPlayerBlocks;
    public GameObject playerBlockPrefab;
    public Transform playerBlockLocation;
    [SerializeField] TurnManager turnManager;
    public Color[] colorIDs;

    private void Start()
    {
        List<TurnActor> list = turnManager.GetUpcomingPlayers();
        for (int i = 0; i < list.Count; i++)
        {
            TurnActor item = list[i];
            GameObject g = Instantiate(playerBlockPrefab, playerBlockLocation);
            upcomingPlayerBlocks.Add(g.GetComponent<NameBlock>());
        }

        UpdatePanel();
    }

    public void UpdatePanel()
    {
        //update Active Player Block
        UpdateBlock(activePlayer, turnManager.GetCurrentActor().player);

        List<TurnActor> upcomingPlayers = turnManager.GetUpcomingPlayers();
        for (int i = 0; i < upcomingPlayers.Count; i++)
        {
            UpdateBlock(upcomingPlayerBlocks[i], upcomingPlayers[i].player);
        }
    }

    private void UpdateBlock(NameBlock nameBlock, Player player)
    {
        nameBlock.SetGradient(player.playerColor);
        nameBlock.SetLabel(player.PlayerName);
    }


}
