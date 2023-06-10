using Nova;
using System.Collections;
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
    public GameObject fightButton;
    public GameObject spinner;
    public TextBlock winningPlayerTextBlock;


    private string p1Name;
    private string p2Name;


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

        BeginBattle(win);
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

        BeginBattle(win);
    }

    private void BeginBattle(bool win)
    {
        winningPlayerTextBlock.Text = "???";
        int rotations = Random.Range(10, 20);
        float offset = (win) ? Random.value * 180.0f : 180.0f + (Random.value * 180.0f);
        //await ObjectTransformUtility.RotateObjectSmooth()
    }

    public void SpinObject(GameObject gameObject, float rotationDegrees, float rotationTime)
    {
        StartCoroutine(SpinCoroutine(gameObject, rotationDegrees, rotationTime));
    }

    private IEnumerator SpinCoroutine(GameObject gameObject, float rotationDegrees, float rotationTime)
    {
        float elapsedTime = 0f;
        float currentRotation = 0f;
        float roationOffset = gameObject.transform.rotation.z;
        float speedMultiplier = 1f;
        float rotationSpeed = rotationDegrees / rotationTime;

        while (elapsedTime < rotationTime)
        {
            float rotationAmount = rotationSpeed * speedMultiplier * Time.deltaTime;
            currentRotation += rotationAmount;

            Quaternion newRotation = Quaternion.Euler(0f, 0f, currentRotation + roationOffset);
            gameObject.transform.rotation = newRotation;

            elapsedTime += Time.deltaTime;
            speedMultiplier = 1f - (elapsedTime / rotationTime);


            if (currentRotation % 360f <= 180)
            {
                ShowPlayer1();
            }
            else
            {
                ShowPlayer2();
            }

            yield return null;
        }

        DisplayWinner();
    }

    private void DisplayWinner()
    {
        // make image and namneplate appear
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
