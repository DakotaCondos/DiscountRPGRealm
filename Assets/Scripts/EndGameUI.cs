using Nova;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private UIBlock2D _backingBlock;
    [SerializeField] private UIBlock2D _imageBlock;
    [SerializeField] private GameObject _laurelBlock;
    [SerializeField] private TextBlock _nameTextBlock;


    [Header("Stats")]
    [SerializeField] private TextBlock _pvmWinsTextBlock;
    [SerializeField] private TextBlock _pvmLossesTextBlock;
    [SerializeField] private TextBlock _pvpWinsTextBlock;
    [SerializeField] private TextBlock _pvpLossesTextBlock;

    [Header("EndGameDetails")]
    [SerializeField] private List<Player> _playerDisplay = new();
    [SerializeField] private Player _winningPlayer;
    [SerializeField] private int _playerDisplayIndex = 0;

    [Header("Buttons")]
    [SerializeField] private GameObject _leftButton;
    [SerializeField] private GameObject _rightButton;

    [Header("Credits")]
    [SerializeField] private GameObject _creditsBlock;
    [SerializeField] private GameObject _creditItemPrefab;
    [SerializeField] private GameObject _nameplatePrefab;
    [SerializeField] private GameObject _squareBlockPrefab;
    [SerializeField] private GameObject _roundBlockPrefab;
    [SerializeField] private Transform _creditNewStartPosition;
    [SerializeField] private Transform _creditContinuingStartPosition;
    public Transform TriggerNextCreditLocation;
    public Transform DestroyCreditLocation;
    [SerializeField] private List<MonsterSO> _monsterSOs;
    [SerializeField] private List<SpaceSO> _spaceSOs;
    [SerializeField] private List<ChanceCardSO> _chanceCardSOs;
    [SerializeField] private List<ItemSO> _itemSOs;
    [SerializeField] private Queue<ScriptableObject> _creditItemQueue = new();

    [Header("CreditItem")]
    [SerializeField] private ScriptableObject _currentCreditItem = null;
    [SerializeField] private bool _useSquareCredit = true;
    [SerializeField] private Queue<Texture2D> _creditItemTextures = new();

    private void Start()
    {
        // Create creditItem Queue
        List<ScriptableObject> combinedList = new();
        combinedList.AddRange(_monsterSOs);
        combinedList.AddRange(_spaceSOs);
        combinedList.AddRange(_chanceCardSOs);
        combinedList.AddRange(_itemSOs);

        // Randomize order and add to queue
        ListShuffler.Shuffle(combinedList);
        combinedList.ForEach(item => _creditItemQueue.Enqueue(item));


    }

    private void OnEnable()
    {
        _winningPlayer = TurnManager.Instance.GetCurrentActor().player;
        _playerDisplay.Add(_winningPlayer);
        MusicManager.Instance.EndGameMusic();

        if (_winningPlayer.TeamID != 0)
        {
            List<Player> winningPlayers = new();
            List<Player> losingPlayers = new();

            foreach (TurnActor actor in TurnManager.Instance.GetUpcomingPlayers())
            {
                if (!actor.isPlayer) { continue; }
                if (actor.player.TeamID == _winningPlayer.TeamID)
                {
                    winningPlayers.Add(actor.player);
                }
                else
                {
                    losingPlayers.Add(actor.player);
                }
            }
            _playerDisplay.AddRange(winningPlayers);
            _playerDisplay.AddRange(losingPlayers);
        }
        else
        {
            foreach (TurnActor actor in TurnManager.Instance.GetUpcomingPlayers())
            {
                if (!actor.isPlayer) { continue; }
                _playerDisplay.Add(actor.player);
            }
        }

        DisplayPlayer(0);
    }

    private void OnDisable()
    {
        MusicManager.Instance.MainGameMusic();

    }

    private void DisplayPlayer(int index)
    {
        Player player = _playerDisplay[index];

        _backingBlock.Color = player.playerColor;
        _imageBlock.SetImage(player.playerTexture);
        _nameTextBlock.Text = player.PlayerName;
        _pvmWinsTextBlock.Text = $"Won: {player.PVMwins}";
        _pvmLossesTextBlock.Text = $"Lost: {player.PVMlosses}";
        _pvpWinsTextBlock.Text = $"Won: {player.PVPwins}";
        _pvpLossesTextBlock.Text = $"Lost:  {player.PVPlosses}";

        if (player.Equals(_winningPlayer))
        {
            _laurelBlock.SetActive(true);
        }
        else if (_winningPlayer.TeamID != 0 && player.TeamID == _winningPlayer.TeamID)
        {
            _laurelBlock.SetActive(true);
        }
        else
        {
            _laurelBlock.SetActive(false);
        }

        _rightButton.SetActive(_playerDisplayIndex + 1 != _playerDisplay.Count);
        _leftButton.SetActive(_playerDisplayIndex != 0);
    }

    public void DisplayNext()
    {
        _playerDisplayIndex++;
        DisplayPlayer(_playerDisplayIndex);
    }

    public void DisplayPrevious()
    {
        _playerDisplayIndex--;
        DisplayPlayer(_playerDisplayIndex);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void TriggerNextCredit()
    {
        if (ApplicationManager.Instance.handlerNotificationsEnabled)
        {
            ConsolePrinter.PrintToConsole($"TriggerNextCredit: {_creditItemQueue.Count} in Queue", Color.green);
        }

        if (_currentCreditItem == null || _creditItemTextures.Count == 0)
        {
            if (_creditItemQueue.Count == 0) { return; }

            _currentCreditItem = _creditItemQueue.Dequeue();
        }

        // Create CreditItem
        GameObject g = Instantiate(_creditItemPrefab, _creditsBlock.transform);
        g.transform.position = (_creditItemTextures.Count == 0) ? _creditNewStartPosition.position : _creditContinuingStartPosition.position;
        CreditItem creditItem = g.GetComponent<CreditItem>();
        creditItem._endGameUI = this;

        if (_creditItemTextures.Count == 0)
        {
            NewCreditSequence(_currentCreditItem, creditItem);
        }
        else
        {
            ContinuingCreditSequence(creditItem);
        }
    }

    private void NewCreditSequence(ScriptableObject currentCreditItem, CreditItem creditItem)
    {
        Type type = currentCreditItem.GetType();

        if (type.Equals(typeof(MonsterSO)))
        {
            _useSquareCredit = false;
            MonsterSO monsterSO = (MonsterSO)currentCreditItem;
            monsterSO.monsterTextures.ForEach(item => _creditItemTextures.Enqueue(item));
            CreateTitleCredit(monsterSO.MonsterName, creditItem);
            return;
        }
        if (type.Equals(typeof(SpaceSO)))
        {
            _useSquareCredit = true;
            SpaceSO spaceSO = (SpaceSO)currentCreditItem;
            spaceSO.spaceTextures.ForEach(item => _creditItemTextures.Enqueue(item));
            CreateTitleCredit(spaceSO.spaceName, creditItem);
            return;
        }
        if (type.Equals(typeof(ChanceCardSO)))
        {
            _useSquareCredit = true;
            ChanceCardSO chanceCardSO = (ChanceCardSO)currentCreditItem;
            chanceCardSO.images.ForEach(item => _creditItemTextures.Enqueue(item));
            CreateTitleCredit(chanceCardSO.cardTitle, creditItem);
            return;
        }
        if (type.Equals(typeof(ItemSO)))
        {
            _useSquareCredit = true;
            ItemSO itemSO = (ItemSO)currentCreditItem;
            // Items have only 1 image
            CreateNamePlate(creditItem._leftBlock, itemSO.itemName);
            CreateSquareBlock(creditItem._rightBlock, itemSO.image);
            _currentCreditItem = null;
            return;
        }
    }

    private void ContinuingCreditSequence(CreditItem creditItem)
    {
        if (_useSquareCredit)
        {
            CreateSquareBlock(creditItem._leftBlock, _creditItemTextures.Dequeue());
            if (_creditItemTextures.Count > 0)
            {
                CreateSquareBlock(creditItem._rightBlock, _creditItemTextures.Dequeue());
            }
            else
            {
                _currentCreditItem = null;
            }
        }
        else
        {
            CreateRoundBlock(creditItem._leftBlock, _creditItemTextures.Dequeue());
            if (_creditItemTextures.Count > 0)
            {
                CreateRoundBlock(creditItem._rightBlock, _creditItemTextures.Dequeue());
            }
            else
            {
                _currentCreditItem = null;
            }
        }
    }

    private void CreateTitleCredit(string title, CreditItem creditItem)
    {
        creditItem._textBlock.Text = $"<size=200%>{title}";
        creditItem._textBlock.Margin.Bottom = 20;
        creditItem.GetComponent<UIBlock2D>().AutoSize.Y = AutoSize.Shrink;
    }

    private void CreateRoundBlock(UIBlock2D uiBlock, Texture2D texture2D)
    {
        GameObject g = Instantiate(_roundBlockPrefab, uiBlock.gameObject.transform);
        g.GetComponent<UIBlock2D>().SetImage(texture2D);

    }
    private void CreateSquareBlock(UIBlock2D uiBlock, Texture2D texture2D)
    {
        GameObject g = Instantiate(_squareBlockPrefab, uiBlock.gameObject.transform);
        g.GetComponent<UIBlock2D>().SetImage(texture2D);

    }
    private void CreateNamePlate(UIBlock2D uiBlock, string name)
    {
        GameObject g = Instantiate(_nameplatePrefab, uiBlock.gameObject.transform);
        g.GetComponent<UIHelper>().TextBlocks[0].Text = name;
    }
}