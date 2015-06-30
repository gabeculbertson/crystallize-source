using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class JobPanelEntryUI : MonoBehaviour, IInitializable<JobRef> {

    public Text jobText;
    public RectTransform wordParent;
    public GameObject requiredWordPrefab;

    List<GameObject> instances = new List<GameObject>();

    public void Initialize(JobRef job) {
        jobText.text = job.GameDataInstance.Name;
        UIUtil.GenerateChildren<PhraseJobRequirementGameData>(job.GameDataInstance.GetPhraseRequirements(), instances, wordParent, GetChild);
    }

    GameObject GetChild(PhraseJobRequirementGameData p) {
        var instance = Instantiate<GameObject>(requiredWordPrefab);
        instance.GetInterface<IInitializable<PhraseJobRequirementGameData>>().Initialize(p);
        return instance;
    }

}