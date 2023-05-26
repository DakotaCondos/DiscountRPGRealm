using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRow : MonoBehaviour
{
    public TextBlock playerNameBlock;
    public TextBlock teamBlock;
    public Player player;

    public void Delete()
    {
        GameManager.Instance.RemovePlayer(player);
        Destroy(gameObject);
    }
}