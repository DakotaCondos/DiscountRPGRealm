using Nova;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GuessingGame : MonoBehaviour
{
    [SerializeField] private int _currentRound = 1; // Current round
    public int totalRounds = 5; // Number of rounds to play
    private bool _hasLost = false;
    [Header("Game Details")]
    [SerializeField] int secretNumber; // Secret number for each round
    [SerializeField] int shownNumber; // Shown number for each round

    [Header("UI Blocks")]
    public TextBlock secretNumberTextBlock;
    public TextBlock shownNumberTextBlock;
    public TextBlock headerTextBlock;
    public TextBlock bodyTextBlock;
    public TextBlock footerTextBlock;
    public UIBlock2D bodyBlock;
    public GameObject upButton;
    public GameObject downButton;
    public GameObject exitButton;
    public GameObject nextRoundButton;
    public UIBlock2D backgroundImageBlock;


    [Header("Response Colors")]
    public Color correctColor;
    public Color incorrectColor;
    public Color defaultColor;
    public Color winColor;

    [Header("Runing Time")]
    public float numberShuffleTime = 4;
    public float buttonRevealTime = 0.5f;
    public float transitionPauseTime = 1.0f;

    [Header("Sounds")]
    public AudioClip winnerSound;
    public AudioClip loserSound;
    public AudioClip correctSound;
    public AudioClip numberShuffleSound;

    [Header("Winnable Items")]
    public List<ItemSO> tier1 = new();
    public List<ItemSO> tier2 = new();
    public List<ItemSO> tier3 = new();
    public List<ItemSO> tier4 = new();


    private void OnEnable() => backgroundImageBlock.SetImage(TurnManager.Instance.GetCurrentActor().player.currentSpace.image);

    public void SetupGame(int rounds)
    {
        _currentRound = 0;
        totalRounds = rounds;
        _hasLost = false;
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.challengePanel);
        StartNewRound();
    }

    public void StartNewRound()
    {
        _currentRound++;

        // Generate a random secret number
        secretNumber = Random.Range(1, 100);

        // Generate a random shown number that is different from the secret number
        shownNumber = Random.Range(1, 100);
        while (shownNumber == secretNumber)
        {
            shownNumber = Random.Range(1, 100);
        }

        NewRoundVisuals();
    }

    private async void NewRoundVisuals()
    {
        secretNumberTextBlock.Text = "?";
        bodyTextBlock.Text = "Guess";
        bodyBlock.Color = defaultColor;

        footerTextBlock.Text = $"Round {_currentRound} of {totalRounds}";

        DisableAllButtons();
        upButton.transform.localScale = Vector3.zero;
        downButton.transform.localScale = Vector3.zero;

        StartCoroutine(TextBlockUtility.RandomNumbersCoroutine(shownNumberTextBlock, (1, 101), shownNumber, numberShuffleTime, numberShuffleSound));
        await Task.Delay((int)((numberShuffleTime + transitionPauseTime) * 1000));

        upButton.SetActive(true);
        downButton.SetActive(true);

        // Create an array of tasks
        Task[] tasks = new Task[2];

        // Start the tasks
        tasks[0] = ObjectTransformUtility.ScaleObjectSmooth(upButton, Vector3.one, buttonRevealTime);
        tasks[1] = ObjectTransformUtility.ScaleObjectSmooth(downButton, Vector3.one, buttonRevealTime);

        // Wait for all tasks to complete
        await Task.WhenAll(tasks);
    }

    public void GuessHigher()
    {
        PostSubmission(secretNumber > shownNumber);
    }

    public void GuessLower()
    {
        PostSubmission(secretNumber < shownNumber);
    }

    public void PostSubmission(bool result)
    {
        upButton.SetActive(false);
        downButton.SetActive(false);
        secretNumberTextBlock.Text = secretNumber.ToString();

        if (result)
        {
            if (_currentRound < totalRounds)
            {
                // setup next round
                bodyTextBlock.Text = "Correct";
                bodyBlock.Color = correctColor;
                nextRoundButton.SetActive(true);
                AudioManager.Instance.PlaySound(correctSound, AudioChannel.SFX);
            }
            else
            {
                // you won
                GiveReward();
                bodyTextBlock.Text = "Winner!";
                bodyBlock.Color = winColor;
                AudioManager.Instance.PlaySound(winnerSound, AudioChannel.SFX);
            }
        }
        else
        {
            // you lose
            _hasLost = true;
            bodyTextBlock.Text = "Incorrect";
            bodyBlock.Color = incorrectColor;
            exitButton.SetActive(true);
            AudioManager.Instance.PlaySound(loserSound, AudioChannel.SFX);
        }
    }

    private void DisableAllButtons()
    {
        upButton.SetActive(false);
        downButton.SetActive(false);
        exitButton.SetActive(false);
        nextRoundButton.SetActive(false);
    }

    [ContextMenu("Give Reward")]
    private void GiveReward()
    {
        Player currentplayer = TurnManager.Instance.GetCurrentActor().player;
        switch (_currentRound)
        {
            case <= 2:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                break;
            case 3:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 1));
                break;
            case 4:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 2));
                break;
            case 5:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 3));
                break;
            case 6:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 3));
                break;
            case 7:
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 3));
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.Item, 1, tier1[Random.Range(0, tier1.Count)]));
                break;
            case 8:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 3));
                currentplayer.effects.Enqueue(new(PlayerEffectType.Item, 1, tier2[Random.Range(0, tier2.Count)]));
                break;
            case 9:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 3));
                currentplayer.effects.Enqueue(new(PlayerEffectType.Item, 1, tier3[Random.Range(0, tier3.Count)]));
                break;
            case >= 10:
                currentplayer.effects.Enqueue(new(PlayerEffectType.Money, _currentRound));
                currentplayer.effects.Enqueue(new(PlayerEffectType.XP, 3));
                currentplayer.effects.Enqueue(new(PlayerEffectType.Item, 1, tier4[Random.Range(0, tier4.Count)]));
                break;
        }

        exitButton.SetActive(true);
    }

    public void Exit()
    {
        bool idkYet = true;
        TurnState.TriggerEndChallenge(TurnManager.Instance.GetCurrentActor(), _hasLost, idkYet);
    }
}
