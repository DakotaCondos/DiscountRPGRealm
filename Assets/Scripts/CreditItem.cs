using Nova;
using UnityEngine;

public class CreditItem : MonoBehaviour
{
    [Header("Setup")]
    public EndGameUI _endGameUI;
    [SerializeField] private bool _triggerNextCredit = true;
    [SerializeField] private Vector3 _localPos;
    private Transform _triggerNext;
    private Transform _triggerDestroy;
    public float ScrollSpeedMultiplier = 3.0f;
    public bool ShouldDestroyOnCompletion = false;

    [Header("Blocks")]
    public TextBlock _textBlock;
    public UIBlock2D _leftBlock;
    public UIBlock2D _rightBlock;
    [SerializeField] private UIBlock2D _bottomTransform;


    private void Start()
    {
        _localPos = transform.localPosition;
        _triggerNext = _endGameUI.TriggerNextCreditLocation;
        _triggerDestroy = _endGameUI.DestroyCreditLocation;
    }

    void Update()
    {
        _localPos = new Vector3(0, _localPos.y + Time.deltaTime * 100 * ScrollSpeedMultiplier, 0);
        transform.localPosition = _localPos;

        if (_triggerNextCredit && _bottomTransform.gameObject.transform.position.y > _triggerNext.position.y)
        {
            _endGameUI.TriggerNextCredit();
            _triggerNextCredit = false;
        }
        if (_bottomTransform.gameObject.transform.position.y > _triggerDestroy.position.y)
        {
            if (ShouldDestroyOnCompletion)
            {
                Destroy(gameObject);
            }
            else
            {
                ResetCreditItem();
            }
        }
    }

    public void ResetCreditItem()
    {
        DestroyAllChildren(_leftBlock.gameObject);
        DestroyAllChildren(_rightBlock.gameObject);
        _textBlock.Text = "";
        gameObject.SetActive(false);
        _endGameUI.AddToInactiveCreditItemPool(this);
    }

    private void DestroyAllChildren(GameObject parentObject)
    {
        int childCount = parentObject.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = parentObject.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

}
