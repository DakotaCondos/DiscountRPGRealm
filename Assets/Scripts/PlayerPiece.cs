using Nova;
using System.Collections;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public UIBlock2D backingBlock;
    public UIBlock2D imageBlock;
    public TextBlock hoverLabel;
    public Player player;
    public float minShadowBlur = 0.2f;
    public float maxShadowBlur = 0.9f;

    private Coroutine colorCoroutine;

    private void Start()
    {
        backingBlock.Color = player.playerColor;
        imageBlock.SetImage(player.playerTexture);
        hoverLabel.Text = player.PlayerName;
    }

    public void ActivePlayerEffects(Player player)
    {
        if (player.Equals(this.player))
        {
            StartColorCycle();
        }
        else if (colorCoroutine != null)
        {
            StopColorCycle();
        }
    }

    #region ShadowColor
    public void SetBackingShadowColor(Color color)
    {
        backingBlock.Shadow.Color = color;
    }

    public void SetBackingShadowBlur(float value)
    {
        backingBlock.Shadow.Blur = value;
    }

    [ContextMenu("StartColorCycle")]
    public void StartColorCycle()
    {
        backingBlock.Shadow.Enabled = true;
        colorCoroutine ??= StartCoroutine(StrobeColorCycleCoroutine());
    }

    [ContextMenu("StopColorCycle")]
    public void StopColorCycle()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
            colorCoroutine = null;
            backingBlock.Shadow.Enabled = false;
        }
    }

    private IEnumerator StrobeColorCycleCoroutine()
    {
        while (true)
        {
            Color color = RainbowColorCycler.GetColor();
            SetBackingShadowColor(color);
            SetBackingShadowBlur(minShadowBlur + (maxShadowBlur - minShadowBlur * RainbowColorCycler.GetStrobe()));
            yield return null;
        }
    }
    #endregion
}