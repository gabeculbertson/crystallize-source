using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialLevel01Script : LevelScript {

    enum TutorialObjective {
        Start,
        GrabWord,
        DropWord,
        Complete
    }

    public int wordID;
    public Transform firstClient;

    GameObject speechBubbleInstance;

    void Awake() {
        //CrystallizeEventManager.main.OnSpeechBubbleOpen += HandleOnSpeechBubbleOpen;
    }

    void HandleOnSpeechBubbleOpen(object sender, PhraseEventArgs e) {
        speechBubbleInstance = sender as GameObject;
        //Debug.Log ("SpeechBubble is: " + sender);
    }

    // Use this for initialization
    IEnumerator Start() {
        CrystallizeEventManager.UI.OnSpeechBubbleOpen += HandleOnSpeechBubbleOpen;

        while (!LevelSystemConstructor.main) {
            yield return null;
        }

        PlayerController.LockMovement(this);
        ObjectiveManager.main.SetObjective(this, false);

        // black out the screen
        var fade = UIFadeEffect.Create();
        fade.transform.SetAsFirstSibling();
        fade.enabled = false;

        // wait for everything to initialize
        yield return null;

        var pi = GameObject.FindObjectOfType<PhraseEntryPanelUI>();
        pi.gameObject.SetActive(false);
        //TutorialCanvas.main.ExperienceUI.gameObject.SetActive (false);
        //TutorialCanvas.main.ConversationUI.IsLocked = true;
        //TutorialCanvas.main.ConversationUI.ShowMessages = false;

        for (float f = 0; f < 0.5f; f += Time.deltaTime * 0.5f) {
            fade.canvasGroup.alpha = 1f - f;
            yield return null;
        }

        // wait for the speech bubble to open
        while (!speechBubbleInstance) {
            yield return null;
        }


        SetMessage("Drag words from speech bubbles to learn them.");


        // while the player has not successfully dragged to the objective...
        while (!ObjectiveManager.main.IsWordFound(wordID)) {
            TutorialCanvas.main.ClearAllIndicators();
            TutorialCanvas.main.CreateUIDragBox(
                speechBubbleInstance.GetComponent<RectTransform>(),
                "Drag words from here...");

            CrystallizeEventManager.UI.OnBeginDragWord += Continue;
            yield return StartCoroutine(WaitForEvent());
            CrystallizeEventManager.UI.OnBeginDragWord -= Continue;

            TutorialCanvas.main.ClearAllIndicators();
            TutorialCanvas.main.CreateUIDragBox(
                TutorialCanvas.main.ObjectiveUI.GetObjective(new PhraseSequenceElement(wordID, 0)),
                "...to here.");

            CrystallizeEventManager.UI.OnDropWord += Continue;
            yield return StartCoroutine(WaitForEvent());
            CrystallizeEventManager.UI.OnDropWord -= Continue;

            TutorialCanvas.main.ClearAllIndicators();

            yield return null;

            if (TutorialCanvas.main.GetRegisteredGameObject("TranslationUI") != null) {
                SetMessage("New words need to be translated before they can be added to the inventory");

                var tuiRectTransform = TutorialCanvas.main.GetRegisteredGameObject("TranslationUI").GetComponent<RectTransform>();
                TutorialCanvas.main.CreateUIDragBox(tuiRectTransform, "Enter translation");

                while (TutorialCanvas.main.GetRegisteredGameObject("TranslationUI") != null) {
                    yield return null;
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        pi.gameObject.SetActive(true);

        SetObjectiveState(TutorialObjective.GrabWord);
        yield return StartCoroutine(RunStateMachine<TutorialObjective>(GetObjectiveState, SetObjectiveState, TutorialObjective.Complete));

        ClearMessages();
        TutorialCanvas.main.ClearAllIndicators();

        for (float f = 0.5f; f < 1f; f += Time.deltaTime * 0.5f) {
            fade.canvasGroup.alpha = 1f - f;
            yield return null;
        }
        fade.canvasGroup.alpha = 0;

        ObjectiveManager.main.SetObjective(this, true);
    }

    TutorialObjective GetObjectiveState() {
        if (PlayerData.Instance.Conversation.GetConversationComplete(firstClient.GetWorldID())) {
            return TutorialObjective.Complete;
        }

        if (UISystem.main.PhraseDragHandler.IsDragging) {
            return TutorialObjective.DropWord;
        }

        return TutorialObjective.GrabWord;
    }

    void SetObjectiveState(TutorialObjective obj) {
        TutorialCanvas.main.ClearAllIndicators();
        switch (obj) {
            case TutorialObjective.GrabWord:
                SetMessage("Click or drag words from the inventory to your speech bubble to complete the conversation");
                TutorialCanvas.main.CreateUIDragBox(
                    TutorialCanvas.main.ObjectiveUI.GetObjective(new PhraseSequenceElement(wordID, 0)),
                    "Drag words from here...");
                break;

            case TutorialObjective.DropWord:
                var inputPanel = GameObject.FindObjectOfType<PhraseEntryPanelUI>();
                TutorialCanvas.main.CreateUIDragBox(
                    inputPanel.GetComponent<RectTransform>(),
                    "...to here.");
                break;
        }
    }

}
