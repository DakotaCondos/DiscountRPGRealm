using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChanceCardType
{
    GainingMoney,
    LosingMoney,
    GainingExperience,
    LosingExperience,
    GainingPower,
    LosingPower,
    ShadowRealm
}

[CreateAssetMenu(fileName = "ChanceCard", menuName = "ScriptableObjects/ChanceCards")]
public class ChanceCardSO : ScriptableObject
{
    public ChanceCardType CardType;
    public string cardTitle;
    public List<Texture2D> images = new();
}
