using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class PhraseSelectionPanelUI : UIPanel, ITemporaryUI<List<PhraseSequence>, PhraseSequence> {

    const string ResourcePath = "UI/PhraseSelectionPanel";
    public static PhraseSelectionPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<PhraseSelectionPanelUI>(ResourcePath);
    }

    public GameObject buttonPrefab;
    public RectTransform buttonParent;

    List<GameObject> instances = new List<GameObject>();

    public event EventHandler<EventArgs<PhraseSequence>> Complete;

    public void Initialize(List<PhraseSequence> param1) {
        transform.SetParent(MainCanvas.main.transform, false);
        UIUtil.GenerateChildren(param1, instances, buttonParent, CreateInstance);
    }

    GameObject CreateInstance(PhraseSequence phrase) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.AddComponent<DataContainer>().Store(phrase);
        bool held = false;
        if (phrase.IsWord) {
            if (PlayerData.Instance.WordStorage.ContainsFoundWord(phrase.Word)) {
                held = true;
            }
        } else {
            if (PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase)) {
                held = true;
            }
        }

        if(phrase.GetText().Trim() == "?"){
            instance.GetComponentInChildren<Text>().text = "(Not sure what to say...)";
            instance.GetComponent<UIButton>().OnClicked += PhraseSelectionPanelUI_OnClicked;
        } else if (held) {
            instance.GetComponentInChildren<Text>().text = phrase.GetText(JapaneseTools.JapaneseScriptType.Romaji);
            instance.GetComponent<UIButton>().OnClicked += PhraseSelectionPanelUI_OnClicked;
        } else {
            instance.GetComponentInChildren<Text>().text = phrase.Translation;
            instance.GetComponent<Image>().color = Color.gray;
        }
        return instance;
    }

    void PhraseSelectionPanelUI_OnClicked(object sender, EventArgs e) {
        Exit(((Component)sender).GetComponent<DataContainer>().Retrieve<PhraseSequence>());
    }

    void Exit(PhraseSequence phrase) {
        Close();
        Complete.Raise(this, new EventArgs<PhraseSequence>(phrase));
    }

}
