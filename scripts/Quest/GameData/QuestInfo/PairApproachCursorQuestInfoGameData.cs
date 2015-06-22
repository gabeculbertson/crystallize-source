using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PairApproachCursorQuestInfoGameData : QuestInfoGameData  {

    public PairApproachCursorQuestInfoGameData()
        : base() {

    }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var o = new List<QuestObjectiveInfoGameData>();
        o.Add(new QuestObjectiveInfoGameData("Wait for your partner to accept the quest"));
        o.Add(new QuestObjectiveInfoGameData("Point at something"));
        o.Add(new QuestObjectiveInfoGameData("Go to where your partner is pointing"));
        //o.Add(new QuestObjectiveInfoGameData("Wait for your partner to go to where you are pointing"));
        return o;
    }

    protected override void Begin() {
        RequestPartnerQuestState();
    }

    public override void ProcessMessage(EventArgs args) {
        if (!IsJoined(args)) {
            return;
        }

        if (TryPoint(args)) {
            CompleteObjective(1);
        }

        if (TrySelfApproach(args)) {
            CompleteObjective(2);
            CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.Instance.PlayerID, GetQuestInstance()));
        }
//
//        if (TryOtherApproach(args)) {
//            CompleteObjective(3);
//            //CrystallizeEventManager.main.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.main.PlayerID, GetQuestInstance()));
//        }
    }

    bool TryPoint(EventArgs args) {
        if (!(args is Cursor3DPositionChangedEventArgs)) {
            return false;
        }

        if (GetQuestInstance().GetObjectiveState(1).IsComplete) {
            return false;
        }

        if (((Cursor3DPositionChangedEventArgs)args).PlayerID == PlayerManager.Instance.PlayerID) {
            return true;
        }

        return false;
    }

    bool TrySelfApproach(EventArgs args) {
        if (!(args is CursorApproachedEventArgs)) {
            return false;
        }

        if (GetQuestInstance().GetObjectiveState(2).IsComplete) {
            return false;
        }

        var caArgs = (CursorApproachedEventArgs)args;
        if (caArgs.ActorPlayerID == PlayerManager.Instance.PlayerID
            && caArgs.CursorPlayerID != PlayerManager.Instance.PlayerID) {
            return true;
        }

        return false;
    }

    bool TryOtherApproach(EventArgs args) {
        if (!(args is QuestStateChangedEventArgs)) {
            return false;
        }

        var qscArgs = (QuestStateChangedEventArgs)args;
        Debug.Log("Got: " + qscArgs.PlayerID + "; " + qscArgs.GetQuestInstance().GetObjectiveState(2).IsComplete);
        if (qscArgs.PlayerID != PlayerManager.Instance.PlayerID
                && qscArgs.GetQuestInstance().GetObjectiveState(2).IsComplete
                && qscArgs.QuestID == QuestID) {
            return true;
        }

        return false;
    }

}
