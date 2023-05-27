using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public UIBlock2D backingBlock;
    public UIBlock2D imageBlock;
    public TextBlock hoverLabel;
    public Player player;

    private void Start()
    {
        backingBlock.Color = player.playerColor;
        imageBlock.SetImage(player.playerTexture);
        hoverLabel.Text = player.PlayerName;
    }
}
