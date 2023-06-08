using Nova;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    private bool canInventory = false;
    private bool canInteract = false;
    private bool hasInteracted = false;
    private bool canFight = false;
    private bool hasFought = false;
    private bool canTrade = false;
    private bool canMove = false;
    private bool canCancelMove = false;
    private bool canEndTurn = false;

    private ActionButtonsPanel actionButtonsPanel = null;
    private TurnManager turnManager = null;
    StatDisplay statDisplay;
    public PanelSwitcher panelSwitcher;
    [Header("Panels")]
    public UIBlock2D mainPanel;
    public UIBlock2D inventoryPanel;
    public UIBlock2D shopPanel;
    public UIBlock2D fightPanelPvP;
    public UIBlock2D fightPanelPvM;
    public UIBlock2D optionsPanel;
    public UIBlock2D chancePanel;
    public UIBlock2D eventPanel;
    public UIBlock2D startPanel;
    public UIBlock2D challengePanel;


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
        statDisplay = FindObjectOfType<StatDisplay>(true);
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
        if (TurnState.TurnStage == TurnStages.BeginTurn)
        {
            hasFought = false;
            hasInteracted = false;
        }
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
        statDisplay.DisplayStats(actor.player.GetPower(), actor.player.GetMovement());

        if (!actor.isPlayer)
        {
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

        if (space.hasMandatoryEvent && (!hasInteracted && actor.player.hasMoved))
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

    public void SetHasInteracted(bool value)
    {
        hasInteracted = value;
    }

    #region ButtonMethods

    public void SelectInventory()
    {
        print("SelectInventory");
    }
    public void SelectInteract()
    {
        print("SelectInteract");
        // Space is either a chance or trap space
        Space space = turnManager.GetCurrentActor().player.currentSpace;
        if (space.spaceType == SpaceType.Chance)
        {
            print("ChanceSpace");
            TurnState.TriggerBeginChance(turnManager.GetCurrentActor());
        }
        else if (space.spaceType == SpaceType.Trap)
        {
            print("TrapSpace");
            TurnState.TriggerBeginChallenge(turnManager.GetCurrentActor());
        }
        else
        {
            // something is not set up correctly
            print("Interact type not id'd");
        }
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
