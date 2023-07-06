using Nova;
using UnityEngine;

public class NameBlock : MonoBehaviour
{
    public TextBlock labelBlock;
    public UIBlock2D containerBlock;
    public UIBlock2D ImageBackingBlock;
    public UIBlock2D imageBlock;

    public void SetPlayer(Player player)
    {
        labelBlock.Text = player.PlayerName;
        containerBlock.Border.Color = player.playerColor;
        if (imageBlock == null) { Debug.LogWarning("imageBlock == null"); }
        if (ImageBackingBlock == null) { Debug.LogWarning("ImageBackingBlock == null"); }
        //ImageBackingBlock.Color = player.playerColor;
        //containerBlock.Gradient.Color = player.playerColor;
        //imageBlock.SetImage(player.playerTexture);
    }
}
