using Nova;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    private bool canInventory = false;
    private bool canInteract = false;
    private bool canFight = false;
    private bool hasFought = false;
    private bool canTrade = false;
    private bool canMove = false;
    private bool canCancelMove = false;
    private bool canEndTurn = false;

    private ActionButtonsPanel actionButtonsPanel = null;
    private TurnManager turnManager = null;

    public PanelSwitcher panelSwitcher;
    [Header("Panels")]
    public UIBlock2D mainPanel;
    public UIBlock2D inventoryPanel;
    public UIBlock2D shopPanel;
    public UIBlock2D fightPanelPvP;
    public UIBlock2D fightPanelPvM;
    public UIBlock2D optionsPanel;

    public static ActionsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        actionButtonsPanel = FindObjectOfType<ActionButtonsPanel>(true);
        turnManager = FindObjectOfType<TurnManager>(true);
    }


    public void SetButtons()
    {
        actionButtonsPanel.EnableInventoryButton(canInventory);
        actionButtonsPanel.EnableInteractButton(canInteract);
        actionButtonsPanel.EnableFightButton(canFight);
        actionButtonsPanel.EnableTradeButton(canTrade);
        actionButtonsPanel.EnableMoveButton(canMove);
        actionButtonsPanel.EnableMoveCancelButton(canCancelMove);
        actionButtonsPanel.EnableEndTurnButton(canEndTurn);
    }

    private void ResetState()
    {
        if (TurnState.TurnStage == TurnStages.BeginTurn) { hasFought = false; }
        canInventory = false;
        canInteract = false;
        canFight = false;
        canTrade = false;
        canMove = false;
        canCancelMove = false;
        canEndTurn = false;
    }

    public void DetermineActions(TurnActor actor)
    {
        ResetState();
        panelSwitcher.SetActivePanel(mainPanel);

        if (!actor.isPlayer)
        {
            {   // enabling end-turn option until monster turns are automated

                Debug.LogWarning("Disable non player actions");
                canEndTurn = true;
            }

            // keep buttons disabled
            SetButtons();
            return;
        }

        Space space = actor.player.currentSpace;

        if (space.hasMonster)
        {
            SetButtons();
            TriggerMonsterFightUI(actor.player, space.monsterAtSpace);
            return;
        }

        if (space.hasMandatoryEvent)
        {
            //perhaps just trigger this automatically in the future
            canInteract = true;
            SetButtons();
            return;
        }

        //if mid movement
        if (TurnState.TurnStage == TurnStages.BeginMovement)
        {
            canCancelMove = true;
            SetButtons();
            return;
        }

        canInventory = true;
        canTrade = space.shopLevel > 0;
        canMove = !actor.player.hasMoved;
        canEndTurn = !canMove;

        // If space has >1 Team0(No Team) players or other teams players
        if (space.spaceType != SpaceType.Town && !hasFought)
        {
            canFight = space.GetFightableEntities(actor.player).Count > 0;
        }

        SetButtons();
    }

    private void TriggerMonsterFightUI(Player player, Monster monsterAtSpace)
    {
        panelSwitcher.SetActivePanel(fightPanelPvM);
        fightPanelPvM.GetComponent<FightMonsterPanelUI>().CreatePvM(player, monsterAtSpace);
    }

    public void SetHasFought(bool value)
    {
        hasFought = value;
    }

    #region ButtonMethods

    public void SelectInventory()
    {
        print("SelectInventory");
    }
    public void SelectInteract()
    {
        print("SelectInteract");
    }
    public void SelectFight()
    {
        print("SelectFight");
        panelSwitcher.SetActivePanel(fightPanelPvP);
        FightPanelUI fp = fightPanelPvP.GetComponent<FightPanelUI>();
        fp.CreatePlayer(turnManager.GetCurrentActor().player);
        List<Player> fightable = turnManager.GetCurrentActor().player.currentSpace.GetFightablePlayers(turnManager.GetCurrentActor().player);
        fp.CreateButtons(fightable);
    }
    public void SelectTrade()
    {
        print("SelectTrade");
    }
    public void SelectMove()
    {
        TurnState.TriggerBeginMovement(turnManager.GetCurrentActor());
    }
    public void SelectCancelMove()
    {
        TurnState.TriggerEndMovement(turnManager.GetCurrentActor(), turnManager.GetCurrentActor().player.currentSpace);
    }
    public void SelectMenu()
    {
        print("SelectMenu");
    }
    public void SelectEndTurn()
    {
        turnManager.NextTurn();
    }
    #endregion
}
