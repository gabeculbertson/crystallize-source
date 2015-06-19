using UnityEngine;
using System.Collections;

public class MoneyQuestReward : QuestReward {

    public int Amount { get; set; }

    public override void GrantReward() {
        PlayerManager.main.playerData.Money += Amount;

        CrystallizeEventManager.PlayerState.RaiseMoneyChanged(this, System.EventArgs.Empty);
    }

    public override string GetRewardDescription() {
        return Amount + "yen";
    }

}
