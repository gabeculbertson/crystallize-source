using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlInclude(typeof(FetchQuestInfoGameData))]
[XmlInclude(typeof(CompleteConversationQuestInfoGameData))]
[XmlInclude(typeof(CompleteBranchQuestInfoGameData))]
[XmlInclude(typeof(LearnWordQuestInfoGameData))]
[XmlInclude(typeof(SeekContextQuestInfoGameData))]
[XmlInclude(typeof(SeekPartnerContextQuestInfoGameData))]
[XmlInclude(typeof(GiveItemQuestInfoGameData))]
[XmlInclude(typeof(HelpGiveItemQuestInfoGameData))]
[XmlInclude(typeof(PerformPairAnimationQuestInfoGameData))]
[XmlInclude(typeof(PerformPairGreetingQuestInfoGameData))]
[XmlInclude(typeof(PerformPairDialogueQuestInfoGameData))]
[XmlInclude(typeof(GetPhraseFromNPCQuestInfoGameData))]
[XmlInclude(typeof(GenericQuestInfoGameData))]
[XmlInclude(typeof(PairApproachCursorQuestInfoGameData))]
public class QuestInfoGameData : ISerializableDictionaryItem<int> {

	public int Key { 
		get {
			return QuestID;
		}
	}

	public int QuestID { get; set; }
	public int WorldID { get; set; }

	public string Title { get; set; }
	public string Description { get; set; }
    public NPCActorLine QuestPromptLine { get; set; }
	public List<QuestObjectiveInfoGameData> Objectives { get; set; }
    public List<StatePrerequisite> Prerequisites { get; set; }
    public List<QuestReward> Rewards { get; set; }

	public QuestInfoGameData(){
		QuestID = -1;
		WorldID = -1;
		Title = "New quest";
		Description = "EMPTY";
        QuestPromptLine = new NPCActorLine();
		Objectives = new List<QuestObjectiveInfoGameData> ();
        Prerequisites = new List<StatePrerequisite>();
        Rewards = new List<QuestReward>();
	}

	public QuestInfoGameData(int questID, int clientID) : this(){
		QuestID = questID;
		WorldID = clientID;
	}

	public List<QuestObjectiveInfoGameData> GetObjectives(){
        if (Objectives.Count == 0) {
            Objectives = GetDefaultObjectives();
        }

        if (PlayerData.IsInitialized) {
            if (PlayerData.Instance.Flags.GetFlag(FlagPlayerData.IsMultiplayer)
                && !GameSettings.GetFlag(GameSystemFlags.LockQuestInterdependence)) {
                var l = new List<QuestObjectiveInfoGameData>(Objectives);
                l.Add(new QuestObjectiveInfoGameData("Help your partner complete the quest"));
                return l;
            }
        }

        return Objectives;
	}

    public virtual List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        return new List<QuestObjectiveInfoGameData>();
    }

    public bool IsAvailable() {
        foreach (var prereq in Prerequisites) {
            if (!prereq.IsFulfilled()) {
                return false;
            }
        }
        return true;
    }

    public void BeginQuest() {
        Begin();

		CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.main.PlayerID, GetQuestInstance()));
    }

    protected virtual void Begin() {  }

	public virtual void ReceiveMessage(System.EventArgs args) { 
		ProcessMessage (args);

		if (PlayerData.Instance.Flags.GetFlag (FlagPlayerData.IsMultiplayer)
            && !GameSettings.GetFlag(GameSystemFlags.LockQuestInterdependence)) {
			if(PartnerQuestComplete(args)){
				CompleteObjective(GetObjectives().Count - 1);
			}
		}

		if (AllObjectivesComplete ()) {
			CompleteQuest();
		}
	}

    public virtual void ProcessMessage(System.EventArgs args) { }

    public QuestInstanceData GetQuestInstance() {
        return PlayerManager.main.playerData.QuestData.GetQuestInstance(QuestID);
    }

    protected bool AllObjectivesComplete() {
        var qi = GetQuestInstance();
        var objs = GetObjectives();
        for (int i = 0; i < objs.Count; i++) {
            if (!qi.GetObjectiveState(i).IsComplete) {
                return false;
            }
        }
        return true;
    }

    protected void CompleteObjective(int index) {
		var qi = GetQuestInstance ();
        if (!qi.GetObjectiveState(index).IsComplete) {
            qi.SetObjectiveState(index, true);
            EffectManager.main.PlayPositiveFeedback();
            EffectManager.main.EnqueueEffect(() => AudioManager.main.PlayPhraseSuccess(), 0.15f);
            DataLogger.LogTimestampedData("ObjectiveComplete", QuestID.ToString(), index.ToString());
        }
        CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.main.PlayerID, qi));
		//CrystallizeEventManager.main.RaiseSendQuestStateRequested (this, new PartnerObjectiveCompleteEventArgs (QuestID));
    }

    protected void CompleteQuest() {
        var qi = GetQuestInstance();
        if (qi.State != ObjectiveState.Complete) {
            qi.State = ObjectiveState.Complete;
            EffectManager.main.PlayMessage("Quest complete!", Color.yellow);
            EffectManager.main.EnqueueEffect(() => AudioManager.main.PlayDialogueSuccess(), 0.2f);
            DataLogger.LogTimestampedData("QuestComplete", QuestID.ToString());
        }
        CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.main.PlayerID, qi));
		CrystallizeEventManager.Network.RaiseSendQuestStateRequested(this, new PartnerObjectiveCompleteEventArgs(QuestID));
    }

    protected void RequestPartnerQuestState() {
        CrystallizeEventManager.PlayerState.RaiseQuestStateRequested(this, new QuestEventArgs(QuestID));
    }

    protected bool IsJoined(System.EventArgs args) {
        var qi = GetQuestInstance();
        if (qi.GetObjectiveState(0).IsComplete) {
            return true;
        } else {
            if (args is QuestStateChangedEventArgs) {
                var qsc = (QuestStateChangedEventArgs)args;
                
                if (qsc.PlayerID != PlayerManager.main.PlayerID 
                    && qsc.QuestState == ObjectiveState.Active 
                    && qsc.QuestID == QuestID) {
                        CompleteObjective(0);
                }
            }
        }
        return false;
    }

	bool PartnerQuestComplete(System.EventArgs args){
		var objIndex = GetObjectives ().Count - 1;
		if (GetQuestInstance ().GetObjectiveState (objIndex).IsComplete) {
			return false;
		}

		if (!(args is QuestStateChangedEventArgs)) {
			return false;
		}
		var qscArgs = (QuestStateChangedEventArgs)args;

		if (qscArgs.PlayerID == PlayerManager.main.PlayerID || qscArgs.QuestID != QuestID) {
			return false;
		}

		var inst = qscArgs.GetQuestInstance ();
		for (int i = 0; i < objIndex; i++) {
			if(!inst.GetObjectiveState(i).IsComplete){
				return false;
			}
		}

		return true;
	}

}
