using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JobPanelUI : UIPanel, ITemporaryUI<object, DaySessionArgs> {

    const string ResourcePath = "UI/JobPanel";
    public static JobPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<JobPanelUI>(ResourcePath);
    }

    public GameObject jobEntryPrefab;
    public RectTransform jobEntryParent;

    public event EventHandler<EventArgs<DaySessionArgs>> Complete;

    bool chooseTask = false;
    List<GameObject> instances = new List<GameObject>();

    public void Initialize(object param1)
    {
        transform.SetParent(MainCanvas.main.transform, false);
        var jobs = (from j in GameData.Instance.Jobs.Items
                    where new JobRef(j.ID).PlayerDataInstance.Shown
                    select new JobRef(j.ID));
        UIUtil.GenerateChildren<JobRef>(jobs, instances, jobEntryParent, CreateChild);
    }

    GameObject CreateChild(JobRef job) {
        var instance = Instantiate<GameObject>(jobEntryPrefab);
        instance.GetInterface<IInitializable<JobRef>>().Initialize(job);
        var b = instance.GetComponentInChildren<UIButton>();
        b.OnClicked += JobPanelUI_OnClicked;
        b.gameObject.AddComponent<DataContainer>().Store(job);
        return instance;
    }

    void JobPanelUI_OnClicked(object sender, EventArgs e) {
        var job  = ((Component)sender).gameObject.GetComponent<DataContainer>().Retrieve<JobRef>();
        var args = new DaySessionArgs("Start", job, chooseTask);

        Exit(args);
    }

    void Exit(DaySessionArgs args) {
        Complete.Raise(this, new EventArgs<DaySessionArgs>(args));
    }

    public void UnlockAllJobs() {
        var jobs = (from j in GameData.Instance.Jobs.Items
                    select new JobRef(j.ID));
        foreach (var j in jobs) {
            PlayerDataConnector.RevealJob(j);
            PlayerDataConnector.UnlockJob(j);
        }
        Initialize(null);
    }

    public void SetChooseTask(bool chooseTask) {
        this.chooseTask = chooseTask;
    }

}