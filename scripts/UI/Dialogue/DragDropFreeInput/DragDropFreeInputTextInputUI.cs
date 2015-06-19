using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DragDropFreeInputTextInputUI : MonoBehaviour {

    const float TransitionSpeed = 5f;
    const int MaxChoices = 10;

    public GameObject optionPrefab;

    InputField input;
    string lastInputValue;
    int selectedOption;
    List<GameObject> optionInstances = new List<GameObject>();

    List<DictionaryDataEntry> optionValues = new List<DictionaryDataEntry>();

    Dictionary<GameObject, int> optionIndicies = new Dictionary<GameObject, int>();

    public event EventHandler<PhraseEventArgs> OnElementChosen;

    void Start() {
        gameObject.SetActive(false);
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

        selectedOption = Mathf.Clamp(selectedOption, 0, optionInstances.Count - 1);
    }

    void UpdateOptions(string entry) {
        ClearOptions();

        entry = entry.ToLower();
        var options = DictionaryData.Instance.FilterEntriesFromRomaji(entry);

        int count = 0;
        foreach (var option in options) {
            var o = AddOption(option);
            optionIndicies[o] = count;
            count++;
            if (count > MaxChoices) {
                break;
            }
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

    GameObject AddOption(DictionaryDataEntry entry) {
        var option = Instantiate(optionPrefab) as GameObject;
        var pse = new PhraseSequenceElement(entry.ID, 0);
        option.transform.SetParent(MainCanvas.main.transform);
        option.transform.position = input.transform.position + (optionInstances.Count * 32f + 40f) * Vector3.up;// +new Vector3(80f, 20f);
        option.GetComponentInChildren<Text>().text = JapaneseTools.KanaConverter.Instance.ConvertToRomaji(entry.Kana); //phrase.ConvertedText;
        option.GetComponent<Image>().color = GUIPallet.main.GetColorForWordCategory(pse.GetPhraseCategory());
        option.GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
        option.GetComponent<UIButton>().OnClicked += HandleOnClicked;

        if (optionInstances.Count == selectedOption) {
            option.transform.localScale = Vector3.one;
            option.GetComponent<CanvasGroup>().alpha = 1f;
        } else {
            option.transform.localScale = 0.75f * Vector3.one;
            option.GetComponent<CanvasGroup>().alpha = 0.75f;
        }

        optionValues.Add(entry);
        optionInstances.Add(option);

        return option;
    }

    void HandleOnClicked(object sender, EventArgs e) {
        var b = sender as UIButton;
        if (b == null) {
            return;
        }

        if (optionIndicies.ContainsKey(b.gameObject)) {
            selectedOption = optionIndicies[b.gameObject];
            AcceptCurrentOption();
        }
    }

    void ClearOptions() {
        foreach (var opt in optionInstances) {
            Destroy(opt);
        }

        optionValues.Clear();
        optionInstances.Clear();
        optionIndicies.Clear();
    }

    void AcceptCurrentOption() {
        if (OnElementChosen != null) {
            var p = new PhraseSequenceElement(optionValues[selectedOption].ID, 0);
            if (OnElementChosen != null) {
                OnElementChosen(this, new PhraseEventArgs(p));
            }
        }

        ClearOptions();
        input.text = "";
    }

}
