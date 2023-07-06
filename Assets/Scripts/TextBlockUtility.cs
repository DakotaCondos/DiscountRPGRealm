using Nova;
using System.Collections;
using UnityEngine;

public static class TextBlockUtility
{
    public static IEnumerator RandomNumbersCoroutine(TextBlock textBlock, (int, int) range, int finalNumber, float totalTime, AudioClip sound = null)
    {
        float ellapsedTime = 0.01f;
        float timeDelay = 0.05f;
        int randomValue = Random.Range(range.Item1, range.Item2);
        while (ellapsedTime < totalTime)
        {
            // Generate a random number in range
            int newRandomValue = Random.Range(range.Item1, range.Item2);

            while (newRandomValue == randomValue)
            {
                newRandomValue = Random.Range(range.Item1, range.Item2);
            }
            randomValue = newRandomValue;

            textBlock.Text = randomValue.ToString();
            if (sound != null) { AudioManager.Instance.PlaySound(sound, AudioChannel.SFX); }

            //timeDelay *= (ellapsedTime / runTime < 0.3f) ? 1.05f : (ellapsedTime / runTime < 0.5f) ? 1.08f : (ellapsedTime / runTime < 0.7f) ? 1.15f : 1.25f;
            timeDelay = 0.05f + .5f * (ellapsedTime / totalTime);
            yield return new WaitForSeconds(timeDelay);


            ellapsedTime += timeDelay; // Update ellapsed time
        }

        // Set the final number
        textBlock.Text = finalNumber.ToString();
        if (sound != null) { AudioManager.Instance.PlaySound(sound, AudioChannel.SFX); }
    }
}