using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBlock : MonoBehaviour
{
    public TextBlock labelBlock;
    public UIBlock2D containerBlock;

    public void SetLabel(string newText)
    {
        labelBlock.Text = newText;
    }

    public void SetGradient(Color color)
    {
        containerBlock.Gradient.Color = color;
    }
}
