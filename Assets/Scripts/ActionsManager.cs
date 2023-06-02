using System.Linq;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    private bool canInventory = false;
    private bool canInteract = false;
    private bool canFight = false;
    private bool canTrade = false;
    private bool canMove = false;
    private bool canEndTurn = false;

    private ActionButtonsPanel actionButtonsPanel = null;
    private TurnManager turnManager = null;

    private void Awake()
    {
        actionButtonsPanel = FindObjectOfType<ActionButtonsPanel>(true);
        turnManager = FindObjectOfType<TurnManager>(true);
        if (actionButtonsPanel == null) { Debug.LogError("ActionsManager could not find ActionButtonsPanel"); }
    }

    public void SetButtons()
    {
        actionButtonsPanel.EnableInventoryButton(canInventory);
        actionButtonsPanel.EnableInteractButton(canInteract);
        actionButtonsPanel.EnableFightButton(canFight);
        actionButtonsPanel.EnableTradeButton(canTrade);
        actionButtonsPanel.EnableMoveButton(canMove);
        actionButtonsPanel.EnableEndTurnButton(canEndTurn);
    }

    private void ResetState()
    {
        canInventory = false;
        canInteract = false;
        canFight = false;
        canTrade = false;
        canMove = false;
        canEndTurn = false;
    }

    public void DetermineActions(TurnActor actor)
    {
        ResetState();

        if (!actor.isPlayer)
        {
            // Enable Nothing
            SetButtons();
            return;
        }

        Space space = actor.player.currentSpace;

        if (space.monsterAtSpace != null)
        {
            canInventory = true;
            canFight = true;
            SetButtons();
            return;
        }

        if (space.hasMandatoryEvent)
        {
            //perhaps just trigger this automatically in the future
            canInteract = true;
            SetButtons();
            return;
        }

        canInventory = true;
        canTrade = space.shopLevel > 0;
        canMove = !actor.player.hasMoved;
        canEndTurn = !canMove;

        // If space has Team0(No Team) players or other teams players
        if (space.spaceType != SpaceType.Town
            && (space.playersAtSpace.Select(x => x.TeamID == 0).Count() > 0
            || space.playersAtSpace.Select(x => x.TeamID != actor.player.TeamID).Count() > 0))
        {
            canFight = true;
        }

        SetButtons();
    }

    #region ButtonMethods

    public void SelectInventory()
    {

    }
    public void SelectInteract()
    {

    }
    public void SelectFight()
    {

    }
    public void SelectTrade()
    {

    }
    public void SelectMove()
    {
        TurnState.TriggerBeginMovement(turnManager.GetCurrentActor());
    }
    public void SelectMenu()
    {

    }
    public void SelectEndTurn()
    {

    }
    #endregion
}
