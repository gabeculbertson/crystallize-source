using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TextEntryDialogueUI : MonoBehaviour {

    const float TransitionSpeed = 5f;

    public GameObject optionPrefab;
    public GameObject entryPrefab;

    public RectTransform entryParent;

    InputField input;
    string lastInputValue;
    int selectedOption;
    List<GameObject> optionInstances = new List<GameObject>();
    List<GameObject> entryInstances = new List<GameObject>();

    List<PhraseSegmentData> optionValues = new List<PhraseSegmentData>();
    List<PhraseSegmentData> entryValues = new List<PhraseSegmentData>();

    public event EventHandler<PhraseEventArgs> OnPhraseInput;

    void Start() {
        input = GetComponentInChildren<InputField>();
    }

    void Update() {
        if (input.text != lastInputValue) {
            if (input.text == "") {
                ClearOptions();
            } else if (input.text[input.text.Length - 1] == ' ') {
                AcceptCurrentOption();
            } else {
                UpdateOptions(input.text);
            }
            lastInputValue = input.text;
        }

        UpdateOptionInput();
        UpdateSelectedOption();
    }

    void OnEnable() {
        ClearAllEntries();
        if (input) {
            input.text = "";
        }
    }

    void OnDisable() {
        PlayerController.UnlockMovement(this);
    }

    void UpdateOptionInput() {
        //Debug.Log (input);
        input.Select();
        input.ActivateInputField();
        input.MoveTextEnd(false);

        PlayerController.LockMovement(this);

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            selectedOption = Mathf.Clamp(selectedOption + 1, 0, optionInstances.Count);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            selectedOption = Mathf.Clamp(selectedOption - 1, 0, optionInstances.Count);
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            if (input.text == "") {
                RemoveLastEntry();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (OnPhraseInput != null) {
                OnPhraseInput(this, new PhraseEventArgs(GetCurrentPhrase()));
            }
            ClearAllEntries();
            if (input) {
                input.text = "";
            }
            //Close();
        }

        selectedOption = Mathf.Clamp(selectedOption, 0, optionInstances.Count - 1);
    }

    public void Close() {
        gameObject.SetActive(false);
        ClearOptions();
    }

    void UpdateOptions(string entry) {
        ClearOptions();

        entry = entry.ToLower();
        var options = (from p in ScriptableObjectDictionaries.main.phraseDictionaryData.Phrases
                       where p.ConvertedText.ToLower().StartsWith(entry) && p.Category != PhraseCategory.Sentence && p.Category != PhraseCategory.Unknown
                       select p);
        //Debug.Log (options.Count ());

        foreach (var option in options) {
            AddOption(option);
        }

        selectedOption = Mathf.Clamp(selectedOption, 0, optionInstances.Count);
    }

    void UpdateSelectedOption() {
        for (int i = 0; i < optionInstances.Count; i++) {
            var opt = optionInstances[i];
            var a = opt.GetComponent<CanvasGroup>().alpha;
            if (i == selectedOption) {
                opt.transform.localScale = Vector3.MoveTowards(opt.transform.localScale, Vector3.one, TransitionSpeed * Time.deltaTime);
                opt.GetComponent<CanvasGroup>().alpha = Mathf.MoveTowards(a, 1f, TransitionSpeed * Time.deltaTime);
            } else {
                opt.transform.localScale = Vector3.MoveTowards(opt.transform.localScale, 0.75f * Vector3.one, TransitionSpeed * Time.deltaTime);
                opt.GetComponent<CanvasGroup>().alpha = Mathf.MoveTowards(a, 0.75f, TransitionSpeed * Time.deltaTime);
            }
        }
    }

    void AddOption(PhraseSegmentData phrase) {
        var option = Instantiate(optionPrefab) as GameObject;
        option.transform.SetParent(MainCanvas.main.transform);
        option.transform.position = input.transform.position + (optionInstances.Count * 32f + 40f) * Vector3.up + new Vector3(80f, 20f);
        option.GetComponentInChildren<Text>().text = phrase.ConvertedText;
        option.GetComponent<Image>().color = GUIPallet.main.GetColorForWordCategory(phrase.Category);
        option.GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;

        if (optionInstances.Count == selectedOption) {
            option.transform.localScale = Vector3.one;
            option.GetComponent<CanvasGroup>().alpha = 1f;
        } else {
            option.transform.localScale = 0.75f * Vector3.one;
            option.GetComponent<CanvasGroup>().alpha = 0.75f;
        }

        optionValues.Add(phrase);
        optionInstances.Add(option);
    }

    void ClearOptions() {
        foreach (var opt in optionInstances) {
            Destroy(opt);
        }

        optionValues.Clear();
        optionInstances.Clear();
    }

    void AcceptCurrentOption() {
        AddEntry(optionValues[selectedOption]);
        ClearOptions();
        input.text = "";
    }

    public void AddEntry(PhraseSegmentData phrase) {
        var entryInstance = Instantiate(entryPrefab) as GameObject;
        entryInstance.transform.SetParent(entryParent);
        entryInstance.GetComponentInChildren<Text>().text = phrase.ConvertedText;
        entryInstance.GetComponent<Image>().color = GUIPallet.main.GetColorForWordCategory(phrase.Category);
        entryInstance.GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;

        entryValues.Add(phrase);
        entryInstances.Add(entryInstance);
    }

    public void RemoveLastEntry() {
        if (entryInstances.Count > 0) {
            Destroy(entryInstances[entryInstances.Count - 1]);

            entryInstances.RemoveAt(entryInstances.Count - 1);
            entryValues.RemoveAt(entryValues.Count - 1);
        }
    }

    public void ClearAllEntries() {
        foreach (var entry in entryInstances) {
            Destroy(entry);
        }

        entryInstances.Clear();
        entryValues.Clear();
    }

    public PhraseSegmentData GetCurrentPhrase() {
        var phrase = ScriptableObject.CreateInstance<Phrase>();
        foreach (var e in entryValues) {
            phrase.phraseSegments.Add(e);
        }
        return phrase;
    }

}