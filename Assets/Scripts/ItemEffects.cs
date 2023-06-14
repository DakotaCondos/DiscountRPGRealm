using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffectType
{
    Power,
    PowerVsPlayer,
    PowerVsMonster,
    Movement,
    Teleport,
}

[Serializable]
public class ItemEffects
{
    public ItemEffectType Type;
    public int value = 0;

    public ItemEffects(ItemEffectType type, int value)
    {
        Type = type;
        this.value = value;
    }
}
