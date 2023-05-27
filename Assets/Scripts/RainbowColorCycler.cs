using UnityEngine;

public class RainbowColorCycler : MonoBehaviour
{
    public float cycleSpeed = 1f; // Speed at which the colors cycle

    public static RainbowColorCycler Instance { get; private set; } // Singleton instance
    private static float hue = 0f; // Current hue value
    private static Color currentColor; // Current color

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            // If not, assign this instance as the Singleton instance
            Instance = this;
        }
        else
        {
            // If another instance already exists, destroy this instance
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Increment the hue based on the cycle speed and time
        hue += cycleSpeed * Time.deltaTime;

        // Keep the hue value within the range [0, 1]
        if (hue > 1f)
        {
            hue -= 1f;
        }

        // Convert the hue to RGB color
        currentColor = Color.HSVToRGB(hue, 1f, 1f);
    }

    public static Color GetColor()
    {
        return currentColor;
    }
}
