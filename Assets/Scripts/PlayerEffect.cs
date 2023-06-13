using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEffectType
{
    XP,
    Power,
    Movement,
    Money,
}

public class PlayerEffect
{
    public PlayerEffectType type;
    public int effectQuantity;

    public PlayerEffect(PlayerEffectType type, int effectQuantity)
    {
        this.type = type;
        this.effectQuantity = effectQuantity;
    }
}