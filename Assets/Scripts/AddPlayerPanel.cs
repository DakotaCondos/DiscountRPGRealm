using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerPanel : MonoBehaviour
{
    public PlayersStartupPanel playersStartupPanel;
    public TextBlock inputTextBlock;
    public int team = 0;
    public string playerName = "";
    public Dropdown dropdown;

    

    public void CreatePlayer()
    {
        if (playerName.Equals(inputTextBlock.Text))
        {
            playerName = playersStartupPanel.GetRandomFunnyName();
        }
        else
        {
            playerName = inputTextBlock.Text;
        }

        playersStartupPanel.CreatePlayer(playerName, team);
        CloseMenu();
    }

    

    public void CloseMenu()
    {
        playersStartupPanel.DestroyMenu();
    }

    public void SetTeam()
    {
        team = dropdown.DropdownOptions.SelectedIndex;
    }


}
