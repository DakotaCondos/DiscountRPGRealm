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
            Destroy(gameObject);
        }
    }
}
