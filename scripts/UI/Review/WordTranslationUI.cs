using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class WordTranslationUI : UIMonoBehaviour {

    public InputField wordInput;
    public Text placeholderText;

    PhraseSequenceElement word;
    RectTransform target;
    Action successCallback;
    Action failureCallback;

    bool isOpen = true;
    bool successful = false;

    float t = 0;

    public void Initialize(PhraseSequenceElement word, RectTransform target, Action successCallback, Action failureCallback) {
        this.word = word;
        this.successCallback = successCallback;
        this.failureCallback = failureCallback;
        this.target = target;

        Debug.Log("initalized");

        PlayerController.LockMovement(this);
    }

	// Use this for initialization
	IEnumerator Start () {
        TutorialCanvas.main.RegisterGameObject("TranslationUI", gameObject);

        yield return null;

        //Debug.Log(target + "; " + target.transform.position);
        transform.position = (Vector2)target.position + target.rect.center + new Vector2(-80f, 16f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            TryEntry();
        }

        if (isOpen) {
            if (!EventSystem.current.alreadySelecting) {
                EventSystem.current.SetSelectedGameObject(wordInput.gameObject);
            }
            wordInput.OnPointerClick(new PointerEventData(EventSystem.current));
        } else {
            t += Time.deltaTime;
            if (successful) {
                if (t < 0.7f) {
                    GetComponent<Image>().color = Vector4.MoveTowards(GetComponent<Image>().color, GUIPallet.main.successColor, 5f * Time.deltaTime);
                } else {
                    canvasGroup.alpha -= Time.deltaTime;
                    if (canvasGroup.alpha <= 0) {
                        Destroy(gameObject);
                    }
                }
            } else {
                canvasGroup.alpha -= Time.deltaTime;
                if (canvasGroup.alpha <= 0) {
                    Destroy(gameObject);
                }
            }
        }
	}

    void OnDestroy() {
        TutorialCanvas.main.UnregisterGameObject("TranslationUI");
        PlayerController.UnlockMovement(this);
    }

    public void Close() {
        isOpen = false;
        PlayerController.UnlockMovement(this);
        TutorialCanvas.main.UnregisterGameObject("TranslationUI");
        if (successful) {
            if (successCallback != null) {
                successCallback();
            }
        } else {
            Debug.Log("Calling failure: " + failureCallback);
            if (failureCallback != null) {
                failureCallback();
            }
        }
    }

    public void Confirm() {
        TryEntry();
    }

    public void ForgetButtonClicked() {
        placeholderText.text = "type <b>" + word.GetTranslation() + "</b>";
        wordInput.text = "";
    }

    void TryEntry() {
        if (IsCorrect()) {
            successful = true;
            AudioManager.main.PlayDialogueSuccess();
            Close();
        } else {
            GetComponent<Image>().color = GUIPallet.main.failureColor;
        }
    }

    bool IsCorrect() {
        var dictEntry = DictionaryData.Instance.GetEntryFromID(word.WordID);
        var wordEntry = GetPlainText(wordInput.text);
        foreach (var e in dictEntry.GetAllTranslations()) {
            if(wordEntry == GetPlainText(e)){
                return true;
            }
        }
        return false;
    }

    string GetPlainText(string str) {
        var s = "";
        foreach (var c in str) {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) {
                s += c.ToString().ToLower();
            }
        }
        return s;
    }

}
