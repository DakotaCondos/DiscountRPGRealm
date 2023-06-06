using UnityEngine;

public static class ConsolePrinter
{
    public static void PrintToConsole(string message, Color textColor)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(textColor);
        string formattedMessage = $"<color=#{hexColor}>{message}</color>";
        Debug.Log(formattedMessage);
    }
}
