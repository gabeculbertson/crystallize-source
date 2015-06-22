using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MessageBoxUI : UIPanel, ITemporaryUI<string> {

    const string ResourcePath = "UI/MessageBox";
    public static MessageBoxUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<MessageBoxUI>(ResourcePath);
    }

    public Text messageText;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(string param1) {
        if (messageText) {
            messageText.text = param1;
        }
        transform.SetParent(MainCanvas.main.transform, false);
        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;
    }

    public override void Close() {
        base.Close();
        CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        RaiseComplete();
    }

    void RaiseComplete() {
        Close();
        Complete.Raise(this, null);
    }

}
