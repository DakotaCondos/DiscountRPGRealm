using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsHandler : MonoBehaviour
{
    [Header("Setup")]
    public UIBlock2D BackingBlock;
    public UIBlock2D ImageBlock;
    public TextBlock NameTextBlock;
    public GameObject effectDescriptionPrefab;
    public Transform effectsLocation;
    public float delayPerEffect = 1;
    public float delayPerPlayer = 2.5f;

    private List<GameObject> effects = new();

    [Header("Icons")]
    public Texture2D powerIcon;
    public Texture2D movementIcon;
    public Texture2D moneyIcon;
    public Texture2D xpIcon;

    [Header("Icons")]
    public AudioClip xpGood;
    public AudioClip xpBad;
    public AudioClip moneyGood;
    public AudioClip moneyBad;
    public AudioClip movementGood;
    public AudioClip movementBad;
    public AudioClip powerGood;
    public AudioClip powerBad;
    public AudioClip itemSound;

    public static PlayerEffectsHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void ResetEffects()
    {
        foreach (GameObject item in effects)
        {
            Destroy(item);
        }
        effects.Clear();
    }

    public IEnumerator HandleEffects(TaskHelper parentHelper)
    {
        TaskHelper helper = new();
        // Process Monsters
        foreach (Monster monster in MonsterManager.Instance.monsters)
        {
            helper.isComplete = false;
            if (monster.effects.Count == 0) { continue; }
            SetupUI(monster);
            StartCoroutine(ProcessQueueRoutineMonster(monster, helper));

            while (!helper.isComplete)
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(delayPerPlayer);
        }

        // Process Players
        foreach (Player player in GameManager.Instance.Players)
        {
            helper.isComplete = false;
            if (player.effects.Count == 0) { continue; }
            SetupUI(player);
            StartCoroutine(ProcessQueueRoutinePlayer(player, helper));

            while (!helper.isComplete)
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(delayPerPlayer);
        }
        parentHelper.isComplete = true;
    }

    private void SetupUI(Player player)
    {
        ResetEffects();
        BackingBlock.Color = player.playerColor;
        ImageBlock.SetImage(player.playerTexture);
        NameTextBlock.Text = player.PlayerName;
    }

    private void SetupUI(Monster monster)
    {
        ResetEffects();
        BackingBlock.Color = Color.red;
        ImageBlock.SetImage(monster.monsterTexture);
        NameTextBlock.Text = monster.MonsterName;
    }


    private IEnumerator ProcessQueueRoutinePlayer(Player player, TaskHelper helper, bool isPlayer = true)
    {
        while (player.effects.Count > 0)
        {
            PlayerEffect currentItem = player.effects.Dequeue();

            ProcessItem(player, currentItem, isPlayer);

            yield return new WaitForSeconds(delayPerPlayer);
        }

        helper.isComplete = true; // Queue processing completed
        // do ending player effects
    }

    private IEnumerator ProcessQueueRoutineMonster(Monster monster, TaskHelper helper)
    {
        while (monster.effects.Count > 0)
        {
            PlayerEffect currentItem = monster.effects.Dequeue();

            ProcessItem(null, currentItem, false);

            yield return new WaitForSeconds(delayPerPlayer);
        }

        helper.isComplete = true; // Queue processing completed
    }

    private void ProcessItem(Player player, PlayerEffect effect, bool updateStat = true)
    {
        ConsolePrinter.PrintToConsole($"Handling Effects", Color.magenta);
        GameObject g = Instantiate(effectDescriptionPrefab, effectsLocation);
        effects.Add(g);
        UIHelper ui = g.GetComponent<UIHelper>();
        ui.TextBlocks[0].Text = (effect.effectQuantity > 0) ? "+" + effect.effectQuantity.ToString() : effect.effectQuantity.ToString();
        AudioClip clip = xpGood; // Set as default

        switch (effect.type)
        {
            case PlayerEffectType.XP:
                ui.UIBlock2Ds[0].SetImage(xpIcon);
                if (updateStat) { player.AddXP(effect.effectQuantity); }
                clip = (effect.effectQuantity > 0) ? xpGood : xpBad;
                break;
            case PlayerEffectType.Power:
                ui.UIBlock2Ds[0].SetImage(powerIcon);
                if (updateStat) { player.AddPower(effect.effectQuantity); }
                clip = (effect.effectQuantity > 0) ? powerGood : powerBad;
                break;
            case PlayerEffectType.Movement:
                ui.UIBlock2Ds[0].SetImage(movementIcon);
                if (updateStat) { player.AddMovement(effect.effectQuantity); }
                clip = (effect.effectQuantity > 0) ? movementGood : movementBad;
                break;
            case PlayerEffectType.Money:
                ui.UIBlock2Ds[0].SetImage(moneyIcon);
                if (updateStat) { player.AddMoney(effect.effectQuantity); }
                clip = (effect.effectQuantity > 0) ? moneyGood : moneyBad;
                break;
            case PlayerEffectType.Item:
                ui.UIBlock2Ds[0].SetImage(effect.itemSO.image);
                ui.UIBlock2Ds[0].CornerRadius = 16;
                if (updateStat) { player.items.Add(new(effect.itemSO)); }
                clip = itemSound;
                break;
            default:
                break;
        }

        AudioManager.Instance.PlaySound(clip, AudioChannel.SFX);
    }
}
