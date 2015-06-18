using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerformPairAnimationQuestInfoGameData : QuestInfoGameData {

    public string Action { get; set; }

    protected override void Begin() {
        CrystallizeEventManager.PlayerState.RaiseQuestStateRequested(this, new QuestEventArgs(QuestID));
    }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var l = new List<QuestObjectiveInfoGameData>();
        l.Add(new QuestObjectiveInfoGameData("Wait for partner to begin the quest"));
        l.Add(new QuestObjectiveInfoGameData("Wave to your partner"));
        l.Add(new QuestObjectiveInfoGameData("Have your partner wave to you"));
        return l;
    }

    public override void ProcessMessage(System.EventArgs args) {
        var qi = GetQuestInstance();

        if (!IsJoined(args)) {
            return;
        }

        if (args is PersonAnimationEventArgs) {
            var paea = (PersonAnimationEventArgs)args;

            if (paea.TargetObject == PlayerManager.main.PlayerGameObject && !qi.GetObjectiveState(1).IsComplete) {
                CompleteObjective(1);
            }

            if (paea.TargetObject != PlayerManager.main.PlayerGameObject && !qi.GetObjectiveState(2).IsComplete) {
                CompleteObjective(2);
            }
        }
    }

}
