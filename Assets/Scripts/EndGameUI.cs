using Nova;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Transform _creditStartLocation;
    [SerializeField] private Transform _creditEndLocation;
    [SerializeField] private List<MonsterSO> _monsterSOs;
    [SerializeField] private List<SpaceSO> _spaceSOs;
    [SerializeField] private List<ChanceCardSO> _chanceCardSOs;
    [SerializeField] private List<ItemSO> _itemSOs;
    [SerializeField] private Queue<ScriptableObject> _creditItemQueue = new();
    public Transform TriggerNextCreditLocation;
    public Transform DestroyCreditLocation;

    [Header("CreditItem")]
    [SerializeField] private ScriptableObject _currentCreditItem = null;
    [SerializeField] private List<Texture2D> _creditItemTextures = new();

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
        ConsolePrinter.PrintToConsole("TriggerNextCredit", Color.green);
        // If currently creating an item


        // Check items are available

        // Create Block
    }
}
