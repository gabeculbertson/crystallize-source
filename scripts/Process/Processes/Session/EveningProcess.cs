using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EveningProcess : TimeSessionProcess<EveningSessionArgs, HomeRef>, IProcess<EveningSessionArgs, HomeRef> {

    public static readonly ProcessFactoryRef<object, TimeSessionArgs> RequestExplore = new ProcessFactoryRef<object,TimeSessionArgs>();

    protected override void AfterLoad() {
        UILibrary.MoneyState.Get(null);
        UILibrary.HomeShopPanel.Get(null);
        var ui = UILibrary.HomeSelectionPanel.Get(null);
        ui.Complete += ui_Complete;
        //RequestExplore.Get(null, ReturnHomeCallback, this);
    }

    void ui_Complete(object sender, EventArgs<HomeRef> e) {
        Exit(e.Data);
    }

    void ReturnHomeCallback(object sender, TimeSessionArgs args) {
        Exit();
    }

}
