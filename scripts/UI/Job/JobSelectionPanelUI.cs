using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobSelectionPanelUI : UIPanel, IPanelItemSelector<JobRef> {

    const string ResourcePath = "UI/JobSelectionPanel";
    public static JobSelectionPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<JobSelectionPanelUI>(ResourcePath);
    }

    public GameObject buttonPrefab;
    public UIButton nullJobButton;
    public RectTransform buttonParent;

    public event EventHandler<EventArgs<JobRef>> OnItemSelected;

    List<GameObject> instances = new List<GameObject>();

    void Start() {
        transform.SetParent(MainCanvas.main.transform, false);
        var c = nullJobButton.gameObject.AddComponent<DataContainer>();
        c.Store<JobRef>(null);
        nullJobButton.OnClicked += HandleJobClicked;
        UIUtil.GenerateChildren(PlayerData.Instance.Jobs.Items, instances, buttonParent, GetJobButtonInstance); 
    }

    GameObject GetJobButtonInstance(JobPlayerData job) {
        var jr = new JobRef(job.JobID);
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = jr.GameDataInstance.Name;
        instance.GetComponent<UIButton>().OnClicked += HandleJobClicked;
        instance.AddComponent<DataContainer>().Store(jr);
        return instance;
    }

    void HandleJobClicked(object sender, EventArgs e) {
        var c = (Component)sender;
        var jr = c.GetComponent<DataContainer>().Retrieve<JobRef>();
        OnItemSelected.Raise(this, new EventArgs<JobRef>(jr));
    }

}
