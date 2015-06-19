using UnityEngine;
using System.Collections;

public class DebugSession : IDaySession {

    public string SessionArea { get; set; }

    public event System.EventHandler OnComplete;

    public void Begin()
    {
        CrystallizeEventManager.OnLoadComplete += OnLevelLoaded;
        Debug.Log("Loading level: " + SessionArea);
        Application.LoadLevel(SessionArea);
    }

    void OnLevelLoaded(object sender, System.EventArgs args)
    {
        var completeSessionButton = GameObjectLabelManager.GetGameObject("CompleteSessionButton");
        completeSessionButton.GetComponent<UIButton>().OnClicked += HandleCompleteButtonClicked;
    }

    void HandleCompleteButtonClicked(object sender, System.EventArgs e)
    {
        if (OnComplete != null)
        {
            OnComplete(this, System.EventArgs.Empty);
        }

        CrystallizeEventManager.OnInitialized -= OnLevelLoaded;
        OnComplete = null;
    }

}
