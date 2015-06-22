using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetPhraseFromNPCQuestInfoGameData : QuestInfoGameData {

    public int TargetGlobalID { get; set; }
    public PhraseSequence Template { get; set; }

    public GetPhraseFromNPCQuestInfoGameData()
        : base() {
            Template = new PhraseSequence();
    }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var l = new List<QuestObjectiveInfoGameData>();
        l.Add(new QuestObjectiveInfoGameData("Do something"));
        l.Add(new QuestObjectiveInfoGameData("Wait for your partner to complete the quest"));
        return l;
    }

    public override void ProcessMessage(System.EventArgs args) {
        if (args is SpeechBubbleRequestedEventArgs) {
            var sArgs = (SpeechBubbleRequestedEventArgs)args;
            if (TargetGlobalID != -1) {
                if (sArgs.Target.GetWorldID() != TargetGlobalID) {
                    return;
                }
            } else {
                if (sArgs.Target.IsHumanControlled()) {
                    return;
                }
            }

            if (!GetQuestInstance().GetObjectiveState(0).IsComplete) {
                if (sArgs.Phrase != null) {
                    if (sArgs.Phrase.FulfillsTemplate(Template)) {
                        CompleteObjective(0);
                        CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.Instance.PlayerID, GetQuestInstance()));
                    }
                }
            }
        }

        if (args is QuestStateChangedEventArgs) {
            var qscArgs = (QuestStateChangedEventArgs)args;
            if (qscArgs.PlayerID == PlayerManager.Instance.PlayerID || qscArgs.QuestID != QuestID) {
                return;
            }

            if (qscArgs.GetQuestInstance().GetObjectiveState(0).IsComplete) {
                CompleteObjective(1);
            }
        }
    }

}
