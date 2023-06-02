using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    public TextBlock powerText;
    public TextBlock moveText;

    public void DisplayStats(int power, int movement)
    {
        powerText.Text = power.ToString();
        moveText.Text = movement.ToString();
    }
}
