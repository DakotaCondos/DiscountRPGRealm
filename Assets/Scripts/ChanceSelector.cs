using UnityEngine;

public enum RewardType
{
    Money,
    Power,
    XP
}

public static class ChanceSelector
{
    public static int minMoneyReward = -10;
    public static int maxMoneyReward = 25;

    public static int minPowerReward = -5;
    public static int maxPowerReward = 10;

    public static int minXPReward = -3;
    public static int maxXPReward = 10;


    public static (RewardType, int) SelectReward()
    {
        int randomRewardIndex = Random.Range(0, 3); // 0: Money, 1: Power, 2: XP

        switch (randomRewardIndex)
        {
            case 0:
                return (RewardType.Money, SelectMoneyReward());
            case 1:
                return (RewardType.Power, SelectPowerReward());
            case 2:
                return (RewardType.XP, SelectXPReward());
            default:
                Debug.LogError("Invalid reward index: " + randomRewardIndex);
                return (RewardType.Money, 0);
        }
    }

    private static int SelectMoneyReward()
    {
        int moneyReward = Random.Range(minMoneyReward, maxMoneyReward + 1);
        if (moneyReward == 0) { moneyReward = 1; }
        return moneyReward;
    }

    private static int SelectPowerReward()
    {
        int powerReward = Random.Range(minPowerReward, maxPowerReward + 1);
        if (powerReward == 0) { powerReward = 1; }
        return powerReward;
    }

    private static int SelectXPReward()
    {
        int xpReward = Random.Range(minXPReward, maxXPReward + 1);
        if (xpReward == 0) { xpReward = 1; }
        return xpReward;
    }
}
