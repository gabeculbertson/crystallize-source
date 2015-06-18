using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PerformPairDialogueQuestInfoGameData : QuestInfoGameData {

    public PhraseSequence QuestionTemplate { get; set; }
    public PhraseSequence ResponseTemplate { get; set; }

    public PerformPairDialogueQuestInfoGameData()
        : base() {
            QuestionTemplate = new PhraseSequence();
            ResponseTemplate = new PhraseSequence();
    }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var l = new List<QuestObjectiveInfoGameData>();
        l.Add(new QuestObjectiveInfoGameData("Wait for your partner to accept the quest"));
        l.Add(new QuestObjectiveInfoGameData("Greet your partner"));
        l.Add(new QuestObjectiveInfoGameData("Have your partner greet you"));
		l.Add(new QuestObjectiveInfoGameData("Answer your partner's question"));
        return l;
    }

    protected override void Begin() {
        //Debug.Log("quest started");
        foreach (var word in QuestionTemplate.PhraseElements) {
            //Debug.Log(word.GetText());
            if (word.ElementType == PhraseSequenceElementType.FixedWord) {
                PlayerManager.main.playerData.WordStorage.AddObjectiveWord(word.WordID);
            }
        }

        foreach (var word in ResponseTemplate.PhraseElements) {
            //Debug.Log(word.GetText());
            if (word.ElementType == PhraseSequenceElementType.FixedWord) {
                PlayerManager.main.playerData.WordStorage.AddObjectiveWord(word.WordID);
            }
        }

        CrystallizeEventManager.UI.RaiseUpdateUI(this, System.EventArgs.Empty);
        CrystallizeEventManager.PlayerState.RaiseQuestStateRequested(this, new QuestEventArgs(QuestID));
    }

    public override void ProcessMessage(System.EventArgs args) {
        if (!IsJoined(args)) {
            return;
        }

        var qi = GetQuestInstance();
        //Debug.Log()

        if (args is InteractionLoggedEventArgs) {
            //Debug.LogWarning("state: " + qi.GetObjectiveState(0).IsComplete + "; " + qi.GetObjectiveState(1).IsComplete);
            if (InteractionLog.main.Entries.Last() == InteractionLogEntry.Break) {
                return;
            }

            if (InteractionLog.main.Entries.Last().Phrase == null) {
                return;
            }

            var logEntry = InteractionLog.main.Entries.Last();
            bool isPrompt = logEntry.Phrase.FulfillsTemplate(QuestionTemplate) && logEntry.Player == PlayerManager.main.PlayerID;

            if (isPrompt) {
                CompleteObjective(1);
                CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.main.PlayerID, qi));
            } else {
                bool isResponse = qi.GetObjectiveState(1).IsComplete && logEntry.Player != PlayerManager.main.PlayerID && logEntry.Phrase.FulfillsTemplate(ResponseTemplate);
                if (isResponse) {
                    CompleteObjective(2);
                    CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.main.PlayerID, qi));
                } else {
                    //qi.SetObjectiveState(0, false);
                    //qi.SetObjectiveState(1, false);
                }
            }
        }

        if (args is QuestStateChangedEventArgs) {
            var qsc = (QuestStateChangedEventArgs)args;
            var questInstance = qsc.GetQuestInstance();
            if (qsc.PlayerID != PlayerManager.main.PlayerID
                && questInstance.GetObjectiveState(2).IsComplete
                && qsc.QuestID == QuestID) {
                    CompleteObjective(3);
            }
        }
    }

}
