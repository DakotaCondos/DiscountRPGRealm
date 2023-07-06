using Nova;
using UnityEngine;

public class CardUI : MonoBehaviour
{
    public TextBlock cardTitle;
    public UIBlock2D cardFront;
    public UIBlock2D cardBack;

    public void SetCardDisplayFront(string title, Texture2D image)
    {
        cardTitle.Text = title;
        cardFront.SetImage(image);
    }

    public void SetCardDisplayBack(Texture2D image)
    {
        cardBack.SetImage(image);
    }
}
