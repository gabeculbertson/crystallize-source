using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BookmarkPanelUI : MonoBehaviour {

    public GameObject wordPrefab;
    public RectTransform wordParent;

    List<GameObject> wordInstances = new List<GameObject>();

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.UI.OnBookmarkChanged += HandleBookmarkChanged;
        TutorialCanvas.main.RegisterGameObject("BookmarkPanel", gameObject);

        Close();
	}

    void Initialize(PhraseSequence phrase) {
        Open();
        ClearElements();

        foreach (var word in phrase.PhraseElements) {
            var instance = Instantiate(wordPrefab) as GameObject;
            instance.transform.SetParent(wordParent);
            instance.GetComponent<SpeechBubbleWordUI>().Initialize(word);
            wordInstances.Add(instance);
        }
    }

    void HandleBookmarkChanged(object sender, BookmarkChangedEventArgs args) {
        if (args.Phrase == null) {
            Close();
        } else {
            Initialize(args.Phrase);
        }
    }

    void ClearElements() {
        foreach (var w in wordInstances) {
            Destroy(w);
        }
        wordInstances.Clear();
    }

    public void Open() {
        gameObject.SetActive(true);
    }

    public void Close() {
        ClearElements();
        gameObject.SetActive(false);
    }

}
