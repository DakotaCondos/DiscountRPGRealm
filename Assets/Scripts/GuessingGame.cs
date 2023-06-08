using Nova;
using System.Threading.Tasks;
using UnityEngine;

public class GuessingGame : MonoBehaviour
{
    private int currentRound = 1; // Current round
    public int totalRounds = 5; // Number of rounds to play
    [Header("Game Details")]
    [SerializeField] int secretNumber; // Secret number for each round
    [SerializeField] int shownNumber; // Shown number for each round

    [Header("UI Blocks")]
    public TextBlock secretNumberTextBlock;
    public TextBlock shownNumberTextBlock;
    public TextBlock headerTextBlock;
    public TextBlock bodyTextBlock;
    public TextBlock footerTextBlock;
    public GameObject upButton;
    public GameObject downButton;

    [Header("Runing Time")]
    public float numberShuffleTime = 4;
    public float buttonRevealTime = 0.5f;


    public void SetupGame(int rounds)
    {
        currentRound = 1;
        totalRounds = rounds;
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.challengePanel);
        StartNewRound();
    }

    private void StartNewRound()
    {
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
        footerTextBlock.Text = $"Round {currentRound} of {totalRounds}";

        upButton.SetActive(false);
        downButton.SetActive(false);
        upButton.transform.localScale = Vector3.zero;
        downButton.transform.localScale = Vector3.zero;

        StartCoroutine(TextBlockUtility.RandomNumbersCoroutine(shownNumberTextBlock, (1, 101), shownNumber, numberShuffleTime));
        await Task.Delay((int)(numberShuffleTime * 1000));

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
        if (currentRound <= totalRounds)
        {
            secretNumberTextBlock.Text = secretNumber.ToString();

            if (secretNumber > shownNumber)
            {
                Debug.Log("Correct! The secret number was higher.");
                bodyTextBlock.Text = "Correct";

                currentRound++;
                if (currentRound > totalRounds)
                {
                    Debug.Log("Congratulations! You won the game.");
                }
                else
                {
                    StartNewRound();
                }
            }
            else
            {
                Debug.Log("Wrong! The secret number was lower.");
                bodyTextBlock.Text = "Incorrect";

                Debug.Log("Game Over. You lost.");
            }
        }
    }

    public void GuessLower()
    {
        if (currentRound <= totalRounds)
        {
            if (secretNumber < shownNumber)
            {
                Debug.Log("Correct! The secret number was lower.");
                bodyTextBlock.Text = "Correct";

                currentRound++;
                if (currentRound > totalRounds)
                {
                    Debug.Log("Congratulations! You won the game.");
                }
                else
                {
                    StartNewRound();
                }
            }
            else
            {
                Debug.Log("Wrong! The secret number was higher.");
                bodyTextBlock.Text = "Incorrect";

                Debug.Log("Game Over. You lost.");
            }
        }
    }
}
