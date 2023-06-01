using UnityEngine;

public class ActionButtonsPanel : MonoBehaviour
{
    [SerializeField] GameObject inventoryButton;
    [SerializeField] GameObject interactButton;
    [SerializeField] GameObject fightButton;
    [SerializeField] GameObject tradeButton;
    [SerializeField] GameObject moveButton;
    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject endTurnButton;

    private void EnableButton(GameObject button, bool value)
    {
        if (button != null)
        {
            button.SetActive(value);
        }
    }

    public void EnableInventoryButton(bool value)
    {
        EnableButton(inventoryButton, value);
    }

    public void EnableInteractButton(bool value)
    {
        EnableButton(interactButton, value);
    }

    public void EnableFightButton(bool value)
    {
        EnableButton(fightButton, value);
    }

    public void EnableTradeButton(bool value)
    {
        EnableButton(tradeButton, value);
    }

    public void EnableMoveButton(bool value)
    {
        EnableButton(moveButton, value);
    }

    public void EnableEndTurnButton(bool value)
    {
        EnableButton(endTurnButton, value);
    }
}
