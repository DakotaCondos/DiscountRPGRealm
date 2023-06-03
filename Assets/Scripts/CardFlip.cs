using System.Collections;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    private bool isFlipped = false; // Flag to track the card's flipped state

    private Coroutine flipCoroutine; // Coroutine reference for flipping animation

    // Method to flip the card
    public void FlipCard()
    {
        if (flipCoroutine != null)
        {
            // Stop any ongoing flipping coroutine
            StopCoroutine(flipCoroutine);
        }

        flipCoroutine = StartCoroutine(FlipCoroutine());
    }

    private IEnumerator FlipCoroutine()
    {
        const float duration = 1.0f; // Duration of the flip animation
        float elapsedTime = 0.0f;

        Quaternion startRotation = gameObject.transform.rotation;
        Quaternion targetRotation;

        if (!isFlipped)
        {
            targetRotation = Quaternion.Euler(0f, 180f, 0f);
            isFlipped = true;
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
            isFlipped = false;
        }

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            gameObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.rotation = targetRotation; // Ensure the final rotation matches the target rotation
    }
}
