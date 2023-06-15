using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReferences : MonoBehaviour
{
    [Header("Icons")]
    public Texture2D powerIcon;
    public Texture2D movementIcon;
    public Texture2D powerMonsterIcon;
    public Texture2D powerPlayerIcon;
    public Texture2D monsterIcon;
    public Texture2D moneyIcon;
    public Texture2D xpIcon;
    public Texture2D unknownIcon;
    public Texture2D teleportIcon;

    public static UIReferences Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
