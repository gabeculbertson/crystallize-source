using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerformPairGreetingQuestInfoGameData : QuestInfoGameData {

    public PhraseSequence Template { get; set; }

    public PerformPairGreetingQuestInfoGameData()
        : base() {
        Template = new PhraseSequence();
    }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var l = new List<QuestObjectiveInfoGameData>();
        l.Add(new QuestObjectiveInfoGameData("Wait for your partner to accept the quest"));
        l.Add(new QuestObjectiveInfoGameData("Greet your partner"));
        l.Add(new QuestObjectiveInfoGameData("Have your partner greet you"));
        return l;
    }

    protected override void Begin() {
        Debug.Log("quest started");
        foreach (var word in Template.PhraseElements) {
            Debug.Log(word.GetText());
            if (word.ElementType == PhraseSequenceElementType.FixedWord) {
                PlayerData.Instance.WordStorage.AddObjectiveWord(word.WordID);
            }
        }

        CrystallizeEventManager.UI.RaiseUpdateUI(this, System.EventArgs.Empty);
        CrystallizeEventManager.PlayerState.RaiseQuestStateRequested(this, new QuestEventArgs(QuestID));
    }

    public override void ProcessMessage(System.EventArgs args) {
        if (!IsJoined(args)) {
            return;
        }

        if (!(args is SpeechBubbleRequestedEventArgs)) {
            return;
        }

        if (!InteractionManager.IsInteractingWithOtherPlayer()) {
            return;
        }

        var sba = (SpeechBubbleRequestedEventArgs)args;

        if (sba.Phrase == null) {
            return;
        }

        if (!sba.Phrase.FulfillsTemplate(Template)) {
            return;
        }

        var qi = GetQuestInstance();

        if (sba.Target.gameObject == PlayerManager.Instance.PlayerGameObject) {
            if (!qi.GetObjectiveState(1).IsComplete) {
                CompleteObjective(1);
            }
        } else if (sba.Target.IsHumanControlled()) {
            if (!qi.GetObjectiveState(2).IsComplete) {
                CompleteObjective(2);
            }
        }
    }

}
