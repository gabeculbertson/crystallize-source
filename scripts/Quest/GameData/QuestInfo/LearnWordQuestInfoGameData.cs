using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LearnWordQuestInfoGameData : QuestInfoGameData {

    public PhraseSequence Word { get; set; }

    public LearnWordQuestInfoGameData()
        : base() {
            Word = new PhraseSequence();
    }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var o = new List<QuestObjectiveInfoGameData>();
        o.Add(new QuestObjectiveInfoGameData("Learn the word"));
        return o;
    }

    protected override void Begin() {

    }

    public override void ProcessMessage(System.EventArgs args) {
        if(PlayerData.Instance.WordStorage.ContainsInventoryWord(Word.PhraseElements[0])){
            CompleteObjective(0);
        }
    }

}
