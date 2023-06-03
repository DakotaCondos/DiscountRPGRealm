using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardMiniGame : MonoBehaviour
{
    private bool isCardCycleRunning = false;
    private IEnumerator cardCycleCoroutine;
    [SerializeField] CardFlip cardFlip;
    [SerializeField] CardUI cardUI;
    public List<Texture2D> cardBacks = new();
    private int currentCardBackIndex = 0;

    // Start the Coroutine
    public void StartCardCycle(float duration)
    {
        if (isCardCycleRunning)
        {
            Debug.Log("Card cycle is already running");
            return;
        }

        cardCycleCoroutine = BeginCardCycle(duration);
        StartCoroutine(cardCycleCoroutine);
    }

    // Stop the Coroutine
    public void StopCardCycle()
    {
        if (!isCardCycleRunning)
        {
            Debug.Log("Card cycle is not currently running");
            return;
        }

        if (cardCycleCoroutine != null)
        {
            StopCoroutine(cardCycleCoroutine);
            cardCycleCoroutine = null;
            isCardCycleRunning = false;
        }
    }

    private IEnumerator BeginCardCycle(float duration)
    {
        isCardCycleRunning = true;
        float timeElapsed = 0f;
        float delay = 0.1f;

        while (timeElapsed < duration)
        {
            CycleCard();

            yield return new WaitForSeconds(delay);
            timeElapsed += delay;
            delay = (timeElapsed / duration) * 0.5f; // Increasing the delay after each call
        }
        CycleCard();
        FlipCard();
        isCardCycleRunning = false;
    }

    private void CycleCard()
    {
        currentCardBackIndex = (currentCardBackIndex + 1 >= cardBacks.Count) ? 0 : currentCardBackIndex + 1;
        cardUI.SetCardDisplayBack(cardBacks[currentCardBackIndex]);
    }

    private void FlipCard()
    {
        cardFlip.FlipCard();
    }
}
