using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPiece : MonoBehaviour
{
    public UIBlock2D backingBlock;
    public UIBlock2D imageBlock;
    public TextBlock hoverLabel;
    public TextBlock powerLabel;
    public Monster monster;

    private void Start()
    {
        backingBlock.Color = Color.red;
        imageBlock.SetImage(monster.monsterTexture);
        hoverLabel.Text = monster.MonsterName;
    }
}
