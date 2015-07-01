using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JobPanelEntryUI : MonoBehaviour, IInitializable<JobRef> {

    public Text jobText;
    public Text eventsText;
    public Image buttonImage;
    public RectTransform wordParent;
    public GameObject textPrefab;
    public GameObject requiredWordPrefab;

    List<GameObject> instances = new List<GameObject>();

    public void Initialize(JobRef job) {
        jobText.text = job.GameDataInstance.Name;
        if (!job.PlayerDataInstance.Unlocked) {
            buttonImage.color = Color.gray;
            eventsText.text = "???";
        } else {
			Debug.Log (eventsText);
            eventsText.text = job.ViewedEventsString();
        }

        var phrases = job.GameDataInstance.GetPhraseRequirements();
        if (phrases.Count() == 0) {
            var instance = Instantiate<GameObject>(textPrefab);
            instance.GetComponentInChildren<Text>().text = "None";
            instance.GetComponentInChildren<Text>().color = Color.gray;
            instance.transform.SetParent(wordParent);
        } else {
            UIUtil.GenerateChildren<PhraseJobRequirementGameData>(phrases, instances, wordParent, GetChild);
        }
    }

    GameObject GetChild(PhraseJobRequirementGameData p) {
        var instance = Instantiate<GameObject>(requiredWordPrefab);
        instance.GetInterface<IInitializable<PhraseJobRequirementGameData>>().Initialize(p);
        return instance;
    }

}