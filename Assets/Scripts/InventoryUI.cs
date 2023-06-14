using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Player")]
    public TextBlock playerNameTextBlock;
    public UIBlock2D backingBlock;
    public UIBlock2D imageBlock;

    [Header("Stats")]
    public TextBlock powerTextBlock;
    public TextBlock movementTextBlock;
    public TextBlock pvmBonusTextBlock;
    public TextBlock pvpBonusTextBlock;

}
