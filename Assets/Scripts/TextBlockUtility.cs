using Nova;
using System.Collections;
using UnityEngine;

public static class TextBlockUtility
{
    public static IEnumerator RandomNumbersCoroutine(TextBlock textBlock, (int, int) range, int finalNumber, float runTime)
    {
        float ellapsedTime = 0;
        float timeDelay = 0;
        while (ellapsedTime < runTime)
        {
            // Generate a random number in range
            int randomValue = Random.Range(range.Item1, range.Item2);
            textBlock.Text = randomValue.ToString();

            timeDelay += 0.05f; // Increase the time delay

            yield return new WaitForSeconds(timeDelay);

            ellapsedTime += timeDelay; // Update ellapsed time
        }

        // Set the final number
        textBlock.Text = finalNumber.ToString();
    }
}

