using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JobPanelUI : UIPanel, ITemporaryUI<object,JobRef> {

    const string ResourcePath = "UI/JobPanel";
    public static JobPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<JobPanelUI>(ResourcePath);
    }

    public GameObject jobEntryPrefab;
    public RectTransform jobEntryParent;

    public event EventHandler<EventArgs<JobRef>> Complete;

    public void Initialize(object param1)
    {
        transform.SetParent(MainCanvas.main.transform, false);
        var jobs = (from j in GameData.Instance.Jobs.Items 
                    select new JobRef(j.ID));
        UIUtil.GenerateChildren<JobRef>(jobs, new List<GameObject>(), jobEntryParent, CreateChild);
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
        Exit(((Component)sender).gameObject.GetComponent<DataContainer>().Retrieve<JobRef>());
    }

    void Exit(JobRef job) {
        Complete.Raise(this, new EventArgs<JobRef>(job));
    }

}