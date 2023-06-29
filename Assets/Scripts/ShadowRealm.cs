using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRealm : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] private GameObject _card1;
    [SerializeField] private GameObject _card2;
    [SerializeField] private GameObject _card3;

    [Header("CurrentGame")]
    [SerializeField] private int _returnCard;

    private void OnEnable()
    {
        CameraController.Instance.snapToOutOfBoundsView = true;
        _returnCard = UnityEngine.Random.Range(0, 3);
    }
    private void OnDisable()
    {
        _card1.GetComponent<CardFlip>().ResetCard();
        _card2.GetComponent<CardFlip>().ResetCard();
        _card3.GetComponent<CardFlip>().ResetCard();

        CameraController.Instance.snapToOutOfBoundsView = false;
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.mainPanel);
        TurnManager.Instance.NextTurn();
    }



    public void SelectCardA() => Select(0);
    public void SelectCardB() => Select(1);
    public void SelectCardC() => Select(2);

    private void Select(int selection)
    {
        GameObject selectedCard = selection switch
        {
            0 => _card1,
            1 => _card2,
            _ => _card3,
        };

        if (selection == _returnCard)
        {
            selectedCard.GetComponent<UIHelper>().TextBlocks[0].Text = "Return to Start";
        }
        else
        {
            selectedCard.GetComponent<UIHelper>().TextBlocks[0].Text = "Remain Here";
        }

        selectedCard.GetComponent<CardFlip>().FlipCard();
    }

    public void CardCallback(int card)
    {
        print($"CalledBack {card}");
    }
}
