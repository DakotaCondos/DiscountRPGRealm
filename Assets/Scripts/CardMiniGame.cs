using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CardMiniGame : MonoBehaviour
{
    [SerializeField] float cycleRateMinTime = 0.05f;
    [SerializeField] float cycleRateMaxTime = 1f;
    [SerializeField] float cycleDelayMultiplier = 0.5f;
    private bool isCardCycleRunning = false;
    private IEnumerator cardCycleCoroutine;
    [SerializeField] CardFlip cardFlip;
    [SerializeField] CardUI cardUI;
    public List<Texture2D> cardBacks = new();
    private int currentCardBackIndex = 0;
    [SerializeField] float cardMovementMultiplier = 1;
    private Vector3 startPos;
    public List<Texture2D> moneyImagesGood = new();
    public List<Texture2D> powerImagesGood = new();
    public List<Texture2D> xpImagesGood = new();


    [SerializeField] AudioClip moveCardSound;

    public async void CreateChanceGame(TurnActor actor)
    {
        Player player = actor.player;
        // determine result and setup minigame
        (RewardType, int) chanceResult = ChanceSelector.SelectReward();
        switch (chanceResult.Item1)
        {
            case RewardType.Money:
                player.AddMoney(chanceResult.Item2);
                break;
            case RewardType.Power:
                player.AddPower(chanceResult.Item2);
                break;
            case RewardType.XP:
                player.AddXP(chanceResult.Item2);
                break;
            default:
                Debug.LogWarning("RewardType not found!");
                break;
        }

        await PlayCardMinigame();
    }


    private void SetupCard(Texture2D image, string cardTitle, Texture2D icon)
    {

    }
    private async Task PlayCardMinigame()
    {
        // Stub out for now
        await Task.Delay(1000);
    }

    // Start the Coroutine
    public void StartCardCycle(float duration)
    {
        if (isCardCycleRunning)
        {
            Debug.Log("Card cycle is already running");
            return;
        }
        startPos = cardUI.transform.position;
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
            MoveCard(true);
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
            MoveCard();
            PlayCardMovementSound();
            yield return new WaitForSeconds(delay);
            timeElapsed += delay;
            delay = Mathf.Clamp((timeElapsed / duration) * cycleDelayMultiplier, cycleRateMinTime, cycleRateMaxTime); // Increasing the delay after each call
        }
        MoveCard(true);
        PlayCardMovementSound();
        CycleCard();
        FlipCard();
        isCardCycleRunning = false;
    }

    private void MoveCard(bool returnToStart = false)
    {
        if (returnToStart)
        {
            cardUI.transform.position = startPos;
            return;
        }

        cardUI.transform.position = startPos + (Vector3)UnityEngine.Random.insideUnitCircle * cardMovementMultiplier;
    }

    private void CycleCard()
    {
        currentCardBackIndex = (currentCardBackIndex + 1 >= cardBacks.Count) ? 0 : currentCardBackIndex + 1;
        cardUI.SetCardDisplayBack(cardBacks[currentCardBackIndex]);
    }

    private void PlayCardMovementSound()
    {
        AudioManager.Instance.PlaySound(moveCardSound, AudioChannel.SFX);
    }

    private void FlipCard()
    {
        cardFlip.FlipCard();
    }

    public T SelectRandomItem<T>(List<T> itemList)
    {
        if (itemList == null || itemList.Count == 0)
        {
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, itemList.Count);
        return itemList[randomIndex];
    }
}
