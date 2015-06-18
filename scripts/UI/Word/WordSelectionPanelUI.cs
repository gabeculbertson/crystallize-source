using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WordSelectionPanelUI : MonoBehaviour {

    public Transform wordParent;
    public GameObject wordButtonPrefab;

    IPhraseDropHandler source;
    Dictionary<UIButton, PhraseSequenceElement> buttonWords = new Dictionary<UIButton, PhraseSequenceElement>();

	// Use this for initialization
	public void Initialize (IPhraseDropHandler wordContainer) {
        source = wordContainer;

        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        foreach (var word in PlayerManager.main.playerData.WordStorage.InventoryElements) {
            if (word == null) {
                continue;
            }

            var instance = Instantiate(wordButtonPrefab) as GameObject;
            instance.transform.SetParent(wordParent);
            instance.GetComponentInChildren<Image>().color = GUIPallet.main.GetColorForWordCategory(word.GetPhraseCategory());
            instance.GetComponentInChildren<Text>().text = word.GetPlayerText();
            instance.GetComponent<UIButton>().OnClicked += HandleClicked;
            buttonWords[instance.GetComponent<UIButton>()] = word;
        }

        TutorialCanvas.main.RegisterGameObject("WordSelector", gameObject);

        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;
	}

    void HandleActorDeparted(object sender, System.EventArgs e) {
        Close();
    }

    void HandleClicked(object sender, System.EventArgs e) {
        if (!(sender is UIButton)) {
            return;
        }

        if (!buttonWords.ContainsKey((UIButton)sender)) {
            return;
        }

        source.AcceptDrop(new WordContainer(buttonWords[(UIButton)sender]));
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            Close();
        }
    }

    public void Close() {
        Destroy(gameObject);
    }

    void OnDestroy() {
        TutorialCanvas.main.UnregisterGameObject("WordSelector");
        CrystallizeEventManager.Environment.OnActorDeparted -= HandleActorDeparted;
    }
	
}
