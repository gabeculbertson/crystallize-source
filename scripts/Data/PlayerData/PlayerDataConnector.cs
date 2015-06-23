using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayerDataConnector {

    public static void UnlockJob(JobRef job) {
        PlayerData.Instance.Jobs.AddItem(new JobPlayerData(job.ID, true));
        //CrystallizeEventManager.PlayerState.raisej(null, null);
    }

    public static void UnlockHome(HomeRef home) {
        PlayerData.Instance.Homes.AddItem(new HomePlayerData(home.ID, true));
        CrystallizeEventManager.PlayerState.RaiseHomesChanged(null, null);
    }

    public static void AddMoney(int amount) {
        PlayerData.Instance.Money += amount;
        CrystallizeEventManager.PlayerState.RaiseMoneyChanged(null, null);
    }

}
