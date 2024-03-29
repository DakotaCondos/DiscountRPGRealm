using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [Header("Player")]
    public UIBlock2D playerBackingBlock;
    public UIBlock2D playerImageBlock;
    public TextBlock playerPowerTextBlock;
    public TextBlock playerTextBlock;

    [Header("Opponent")]
    public UIBlock2D opponentBackingBlock;
    public UIBlock2D opponentImageBlock;
    public TextBlock opponentPowerTextBlock;
    public TextBlock opponentTextBlock;

    [Header("Visuals")]
    public GameObject continueButton;
    public GameObject spinner;
    public UIBlock2D spinnerChanceBlock;
    public TextBlock winningPlayerTextBlock;

    [Header("Spinner")]
    public float rotationTime = 5f;
    public int rotationsMin = 15;
    public int rotationsMax = 30;
    public AnimationCurve animationCurve;

    [Header("Misc")]
    public List<AudioClip> winSounds;
    public List<AudioClip> LoseSounds;
    public BattleDTO battleDTO;
    private string p1Name;
    private string p2Name;

    public UIBlock2D backgroundImageBlock;


    private void OnEnable() => backgroundImageBlock.SetImage(TurnManager.Instance.GetCurrentActor().player.currentSpace.image);

    public void InitiateBattleSequence(BattleDTO battleDTO)
    {
        this.battleDTO = battleDTO;
        if (battleDTO.playerOpponent != null)
        {
            BuildBattleUI(battleDTO.player, battleDTO.playerOpponent, battleDTO.win);
        }
        else
        {
            BuildBattleUI(battleDTO.player, battleDTO.monster, battleDTO.win);
        }
    }

    public void BuildBattleUI(Player p1, Player p2, bool win)
    {
        playerBackingBlock.Color = p1.playerColor;
        playerImageBlock.SetImage(p1.playerTexture);
        playerPowerTextBlock.Text = p1.GetPowerVsPlayer().ToString();
        playerTextBlock.Text = p1.PlayerName;
        p1Name = p1.PlayerName;

        opponentBackingBlock.Color = p2.playerColor;
        opponentImageBlock.SetImage(p2.playerTexture);
        opponentPowerTextBlock.Text = p2.GetPowerVsPlayer().ToString();
        opponentTextBlock.Text = p2.PlayerName;
        p2Name = p2.PlayerName;

        float winDegrees = 360f * ((float)p1.GetPowerVsPlayer() / (float)(p1.GetPowerVsPlayer() + p2.GetPowerVsPlayer()));
        spinnerChanceBlock.RadialFill.FillAngle = winDegrees;
        BeginBattle(win, winDegrees);
    }

    public void BuildBattleUI(Player p1, Monster p2, bool win)
    {
        playerBackingBlock.Color = p1.playerColor;
        playerImageBlock.SetImage(p1.playerTexture);
        playerPowerTextBlock.Text = p1.GetPowerVsMonster().ToString();
        playerTextBlock.Text = p1.PlayerName;
        p1Name = p1.PlayerName;

        opponentBackingBlock.Color = Color.red;
        opponentImageBlock.SetImage(p2.monsterTexture);
        opponentPowerTextBlock.Text = p2.power.ToString();
        opponentTextBlock.Text = p2.MonsterName;
        p2Name = p2.MonsterName;

        float winDegrees = 360f * ((float)p1.GetPowerVsMonster() / (float)(p1.GetPowerVsMonster() + p2.power));
        spinnerChanceBlock.RadialFill.FillAngle = winDegrees;
        BeginBattle(win, winDegrees);
    }

    private void BeginBattle(bool win, float winDegrees)
    {
        continueButton.SetActive(false);
        winningPlayerTextBlock.Text = "???";
        int rotations = Random.Range(rotationsMin, rotationsMax);
        float offset = (win) ? Random.value * winDegrees : winDegrees + (Random.value * (360f - winDegrees));
        spinner.transform.rotation = Quaternion.identity;
        SpinObject(spinner, (rotations * 360f) + offset, rotationTime, winDegrees, win);
    }

    public void SpinObject(GameObject gameObject, float rotationDegrees, float rotationTime, float winDegrees, bool win)
    {
        StartCoroutine(SpinCoroutine(gameObject, rotationDegrees, rotationTime, winDegrees, win));
    }

    private IEnumerator SpinCoroutine(GameObject gameObject, float rotationDegrees, float rotationTime, float winDegrees, bool win)
    {
        float elapsedTime = 0f;
        float targetRotation = rotationDegrees;

        while (elapsedTime < rotationTime)
        {
            float time = elapsedTime / rotationTime;
            // Use Animation Curve to determine the completion percentage
            float percentComplete = animationCurve.Evaluate(time);
            float currentRotation = Mathf.Lerp(0, targetRotation, percentComplete);

            Quaternion newRotation = Quaternion.Euler(0f, 0f, currentRotation);
            gameObject.transform.rotation = newRotation;

            elapsedTime += Time.deltaTime;

            if (currentRotation % 360f <= winDegrees)
            {
                ShowPlayer1();
            }
            else
            {
                ShowPlayer2();
            }

            yield return null;
        }

        // Make sure the final rotation is exactly the target rotation
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

        continueButton.SetActive(true);
        // Play Sound
        var clipList = (win) ? winSounds : LoseSounds;
        AudioManager.Instance.PlaySound(clipList[Random.Range(0, clipList.Count)], AudioChannel.SFX);
    }


    public void FinishBattle()
    {
        // UI effects go here
        CombatManager.Instance.BattleDTOHandler(battleDTO);
    }

    private void ShowPlayer1()
    {
        winningPlayerTextBlock.Text = p1Name;
    }
    private void ShowPlayer2()
    {
        winningPlayerTextBlock.Text = p2Name;
    }
}
