using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEffectType
{
    XP,
    Power,
    Movement,
    Money,
    Item,
}

public class PlayerEffect
{
    public PlayerEffectType type;
    public int effectQuantity;
    public ItemSO itemSO;

    public PlayerEffect(PlayerEffectType type, int effectQuantity, ItemSO itemSO = null)
    {
        this.type = type;
        this.effectQuantity = effectQuantity;
        this.itemSO = itemSO;
    }
}