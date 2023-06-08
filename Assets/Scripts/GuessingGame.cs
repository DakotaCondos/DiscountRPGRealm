using Nova;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GuessingGame : MonoBehaviour
{
    private int currentRound = 1; // Current round
    public int totalRounds = 5; // Number of rounds to play
    private bool hasLost = false;
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

    public void SetupGame(int rounds)
    {
        currentRound = 0;
        totalRounds = rounds;
        hasLost = false;
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.challengePanel);
        StartNewRound();
    }

    public void StartNewRound()
    {
        currentRound++;

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

        footerTextBlock.Text = $"Round {currentRound} of {totalRounds}";

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
            if (currentRound < totalRounds)
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
                exitButton.SetActive(true);
                AudioManager.Instance.PlaySound(winnerSound, AudioChannel.SFX);
            }
        }
        else
        {
            // you lose
            hasLost = true;
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

    private void GiveReward()
    {
        print("RewardGoesHere");
    }

    public void Exit()
    {
        bool idkYet = true;
        TurnState.TriggerEndChallenge(TurnManager.Instance.GetCurrentActor(), hasLost, idkYet);
    }
}
