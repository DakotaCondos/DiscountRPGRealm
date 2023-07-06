using Nova;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardMiniGame : MonoBehaviour
{
    [SerializeField] float cycleRateMinTime = 0.05f;
    [SerializeField] float cycleRateMaxTime = 1f;
    [SerializeField] float cycleDelayMultiplier = 0.5f;
    [SerializeField] float gameTime = 5f;
    private bool isCardCycleRunning = false;
    private IEnumerator cardCycleCoroutine;
    [SerializeField] CardFlip cardFlip;
    [SerializeField] CardUI cardUI;
    public List<Texture2D> cardBacks = new();
    private int currentCardBackIndex = 0;
    [SerializeField] float cardMovementMultiplier = 1;
    public List<ChanceCardSO> chanceCardSOs = new();
    public Texture2D moneyIcon;
    public Texture2D powerIcon;
    public Texture2D xpIcon;

    public GameObject outcomeDisplay;
    public UIBlock2D rewardTypeIcon;
    public TextBlock rewardDescription;

    private (RewardType, int) chanceResult;

    [SerializeField] AudioClip moveCardSound;
    [SerializeField] AudioClip goodSound;
    [SerializeField] AudioClip badSound;
    [SerializeField] float revealTime = 1f;
    [SerializeField] CameraController cameraController;
    [SerializeField] Transform cardTransform;
    [SerializeField] GameObject responseBlock;
    [SerializeField] GameObject shadowRealmResponseBlock;
    public void CreateChanceGame(TurnActor actor)
    {
        outcomeDisplay.SetActive(false);
        Player player = actor.player;
        ChanceCardSO chanceCardSO;
        // determine result and setup minigame
        chanceResult = ChanceSelector.SelectReward();
        ChanceCardType cardType;
        switch (chanceResult.Item1)
        {
            case RewardType.Money:
                player.AddMoney(chanceResult.Item2);
                cardType = (chanceResult.Item2 > 0) ? ChanceCardType.GainingMoney : ChanceCardType.LosingMoney;
                break;
            case RewardType.Power:
                player.AddPower(chanceResult.Item2);
                cardType = (chanceResult.Item2 > 0) ? ChanceCardType.GainingPower : ChanceCardType.LosingPower;
                break;
            case RewardType.XP:
                player.AddXP(chanceResult.Item2);
                cardType = (chanceResult.Item2 > 0) ? ChanceCardType.GainingExperience : ChanceCardType.LosingExperience;
                break;
            case RewardType.ShadowRealm:
                cardType = ChanceCardType.ShadowRealm;
                break;
            default:
                Debug.LogWarning("RewardType not found!");
                cardType = (chanceResult.Item2 > 0) ? ChanceCardType.GainingMoney : ChanceCardType.LosingMoney;
                break;
        }
        chanceCardSO = SelectRandomItem(chanceCardSOs.Where(card => card.CardType == cardType).ToList());
        SetupCard(chanceCardSO);
        SetupResult();
        PlayCardMinigame();
    }

    private void SetupResult()
    {
        if (chanceResult.Item1 == RewardType.ShadowRealm)
        {
            responseBlock.SetActive(false);
            shadowRealmResponseBlock.SetActive(true);

            ActionsManager.Instance.sendToShadowRealm = true;

            return;
        }

        responseBlock.SetActive(true);
        shadowRealmResponseBlock.SetActive(false);

        Texture2D image = (chanceResult.Item1 == RewardType.Money) ? moneyIcon :
            (chanceResult.Item1 == RewardType.Power) ? powerIcon : xpIcon;

        rewardTypeIcon.SetImage(image);

        rewardDescription.Text = (chanceResult.Item2 > 0) ? $"+{chanceResult.Item2}" : chanceResult.Item2.ToString();
    }

    private void SetupCard(ChanceCardSO chanceCardSO)
    {
        cardUI.SetCardDisplayFront(chanceCardSO.name, SelectRandomItem(chanceCardSO.images));
    }

    private void PlayCardMinigame()
    {
        cardFlip.ResetCard();
        // Switch screen here
        ActionsManager.Instance.panelSwitcher.SetActivePanel(ActionsManager.Instance.chancePanel);
        cameraController.snapToOutOfBoundsView = true;

        // Stub out for now
        StartCardCycle(gameTime);
    }

    public async void DisplayResult()
    {
        outcomeDisplay.transform.localScale = Vector3.zero;
        outcomeDisplay.SetActive(true);
        await ObjectTransformUtility.ScaleObjectSmooth(outcomeDisplay, Vector3.one, revealTime);
        AudioClip SoundClip = (chanceResult.Item2 > 0) ? goodSound : badSound;
        AudioManager.Instance.PlaySound(SoundClip, AudioChannel.SFX);
    }

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
        yield return new WaitForSeconds(0.5f);
        FlipCard();
        isCardCycleRunning = false;
    }

    private void MoveCard(bool returnToStart = false)
    {
        if (returnToStart)
        {
            cardUI.transform.position = cardTransform.position;
            return;
        }

        cardUI.transform.position = cardTransform.position + (Vector3)UnityEngine.Random.insideUnitCircle * cardMovementMultiplier;
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

    public void EndMiniGame()
    {
        TurnState.TriggerEndChance(TurnManager.Instance.GetCurrentActor());
    }
}
