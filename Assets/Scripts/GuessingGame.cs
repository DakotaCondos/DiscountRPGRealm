using Nova;
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



    public void SetupGame(int rounds)
    {
        currentRound = 1;
        totalRounds = rounds;
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

        Debug.Log("Round " + currentRound);
        Debug.Log("Secret Number: " + secretNumber);
        Debug.Log("Shown Number: " + shownNumber);
    }

    public void GuessHigher()
    {
        if (currentRound <= totalRounds)
        {
            if (secretNumber > shownNumber)
            {
                Debug.Log("Correct! The secret number was higher.");
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
                Debug.Log("Game Over. You lost.");
            }
        }
    }
}
