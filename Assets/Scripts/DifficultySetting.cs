using Nova;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DifficultySetting : MonoBehaviour
{
    [SerializeField] private GameDifficulty _currentDifficulty;
    [SerializeField] private List<GameDifficulty> _allDifficulties = new();
    [SerializeField] private int _index;

    [Header("UI")]
    [SerializeField] private GameObject _upButton;
    [SerializeField] private GameObject _downButton;
    [SerializeField] private TextBlock _difficultyText;


    private void Awake()
    {
        _allDifficulties.AddRange(Enum.GetValues(typeof(GameDifficulty)));
    }

    private void OnEnable()
    {
        _currentDifficulty = GameManager.Instance.gameDifficulty;

        for (int i = 0; i < _allDifficulties.Count; i++)
        {
            if (_currentDifficulty.Equals(_allDifficulties[i]))
            {
                _index = i;
                return;
            }
        }
        UpdateSettings();
    }

    public void IncreaseDifficulty()
    {
        _index++;
        UpdateSettings();
    }

    public void DecreaseDifficulty()
    {
        _index--;
        UpdateSettings();
    }

    private void UpdateSettings()
    {
        _currentDifficulty = _allDifficulties[_index];
        _difficultyText.Text = _currentDifficulty.ToString();
        _upButton.SetActive(_index < _allDifficulties.Count - 1);
        _downButton.SetActive(_index > 0);
        GameManager.Instance.gameDifficulty = _currentDifficulty;
    }

}
