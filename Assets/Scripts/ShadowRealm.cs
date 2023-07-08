using Nova;
using System.Threading.Tasks;
using UnityEngine;

public class ShadowRealm : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] private GameObject _card1;
    [SerializeField] private GameObject _card2;
    [SerializeField] private GameObject _card3;

    [Header("Buttons")]
    [SerializeField] private GameObject _buttonsRow;

    [Header("Background")]
    public UIBlock2D backgroundImageBlock;


    [Header("CurrentGame")]
    [SerializeField] private int _returnCard;

    private void OnEnable()
    {
        CameraController.Instance.SetFocusObject(GameBoard.Instance.OutOfBoundsSnapPoint);
        Camera.main.orthographic = false;
        _returnCard = UnityEngine.Random.Range(0, 3);
        _buttonsRow.SetActive(true);
        backgroundImageBlock.SetImage(TurnManager.Instance.GetCurrentActor().player.currentSpace.image);

    }
    private void OnDisable()
    {
        _card1.GetComponent<CardFlip>().ResetCard();
        _card2.GetComponent<CardFlip>().ResetCard();
        _card3.GetComponent<CardFlip>().ResetCard();

        CameraController.Instance.ClearFocusObject(TurnManager.Instance.GetCurrentActor().player.currentSpace.gameObject);
        Camera.main.orthographic = true;
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.mainPanel);
    }



    public void SelectCardA() => Select(0);
    public void SelectCardB() => Select(1);
    public void SelectCardC() => Select(2);

    private void Select(int selection)
    {
        _buttonsRow.SetActive(false);

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

    public async void CardCallback(int card)
    {
        await Task.Delay(2000);

        if (card == _returnCard)
        {
            gameObject.SetActive(false);
            GameBoard.Instance.StartingSpace.SelectSpace();
        }
        else
        {
            gameObject.SetActive(false);
            TurnManager.Instance.NextTurn();
        }

    }
}
