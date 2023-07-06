using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsItem : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private EndGameUI _endGameUI;
    [SerializeField] private bool _triggerNextCredit = true;
    [SerializeField] private Vector3 _localPos;
    private Transform _triggerNext;
    private Transform _triggerDestroy;

    [Header("Blocks")]
    [SerializeField] private TextBlock _textBlock;
    [SerializeField] private UIBlock2D _leftBlock;
    [SerializeField] private UIBlock2D _rightBlock;
    [SerializeField] private UIBlock2D _bottomTransform;


    private void Start()
    {
        _localPos = transform.localPosition;
        print(_localPos);
        _triggerNext = _endGameUI.TriggerNextCreditLocation;
        _triggerDestroy = _endGameUI.DestroyCreditLocation;
    }

    void Update()
    {
        // Move Credit Up
        _localPos = new Vector3(0, _localPos.y + Time.deltaTime * 100, 0);
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
