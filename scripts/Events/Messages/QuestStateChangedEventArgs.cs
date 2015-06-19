using UnityEngine;
using System.Collections;
using Util.Serialization;

[System.Serializable]
public class QuestStateChangedEventArgs : System.EventArgs {

    public int PlayerID { get; set; }
	public int QuestID { get; set; }
    public ObjectiveState QuestState { get; set; }
	public string QuestData { get; set; }


    public QuestStateChangedEventArgs(int playerID, QuestInstanceData questInstace)  {
        PlayerID = playerID;
        //Debug.Log(questInstace.QuestID);
        QuestID = questInstace.QuestID;
        QuestState = questInstace.State;
		QuestData = Serializer.GetBytesForObject<QuestInstanceData> (questInstace);
    }

	public QuestInstanceData GetQuestInstance(){
		return Serializer.GetObjectForBytes<QuestInstanceData>(QuestData);
	}

}
