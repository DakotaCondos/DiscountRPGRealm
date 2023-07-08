using UnityEngine;

public class RainbowColorCycler : MonoBehaviour
{
    public float colorCycleSpeed = 1f; // Speed at which the colors cycle
    public float strobeCycleSpeed = 3f; // Speed at which the strobe cycles

    public static RainbowColorCycler Instance { get; private set; } // Singleton instance
    private static float hue = 0f; // Current hue value
    private static Color currentColor; // Current color

    private static float strobe = 0f; // Current hue value

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
        // Increment the hue and strobe based on the cycle speed and time
        hue = Mathf.Repeat(hue + colorCycleSpeed * Time.deltaTime, 1f);
        strobe = Mathf.Repeat(strobe + strobeCycleSpeed * Time.deltaTime, 1f);

        // Convert the hue to RGB color
        currentColor = Color.HSVToRGB(hue, 1f, 1f);
    }

    public static Color GetColor()
    {
        return currentColor;
    }

    public static float GetStrobe()
    {
        return strobe;
    }
}
