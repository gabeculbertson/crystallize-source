using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeekPartnerContextQuestInfoGameData : QuestInfoGameData {

	public string ContextLabel { get; set; }
	public PhraseSequence ValidExpression { get; set; }

	public SeekPartnerContextQuestInfoGameData() : base(){	
        ValidExpression = new PhraseSequence();
    }

    public SeekPartnerContextQuestInfoGameData(int questID, int clientID)
        : base(questID, clientID) {
		ContextLabel = "NULL";
		Description = "Find out about your partner.";
	}

	public override List<QuestObjectiveInfoGameData> GetDefaultObjectives(){
        var o = new List<QuestObjectiveInfoGameData>();
		o.Add (new QuestObjectiveInfoGameData("Seek the information"));

		return o;
	}

    public override void ProcessMessage(System.EventArgs args) {
        if (!(args is PartnerSaidPhraseEventArgs))
            return;
        var pcArgs = (PartnerSaidPhraseEventArgs)args;
        Debug.Log("Phrase received: " + pcArgs.Phrase.GetText() + " Is valid? " + pcArgs.Phrase.FulfillsTemplate(ValidExpression));

        if (pcArgs.PlayerGuid == Network.player.guid) {
            Debug.Log("guid not right.");
            return;
        }
        if (!pcArgs.Phrase.FulfillsTemplate(ValidExpression)) {
            //Debug.Log("guid not right.");
            return;
        }

        CompleteObjective(0);
        CompleteQuest();
    }

}
