using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugTimeProcess : MonoBehaviour {
	public StandardScriptableObjectSessionData morningSession;
	public StandardScriptableObjectSessionData daySession;
	public StandardScriptableObjectSessionData eveningSession;
	public StandardScriptableObjectSessionData nightSession;
	// Use this for initialization
	void Start () {
		// construct a job reference
		var myJob = new CodedJobRef (10, CreateGameData(), CreatePlayerData());

		var p = GameTimeProcess.GetTestInstance (myJob);
		p.morningSession = morningSession;
		p.daySession = daySession;
		p.eveningSession = eveningSession;
		p.nightSession = nightSession;
	}

	JobGameData CreateGameData(){
		JobGameData ret = new JobGameData ();
		ret.Tasks.Add(CreateTask());
		return ret;
	}

	JobPlayerData CreatePlayerData(){
		var data = new JobPlayerData ();
		data.Unlocked = true;
		data.JobID = 10;
		return data;
	}

	//actually create an instance of the subclass of JobTaskGameData
	JobTaskGameData CreateTask(){
		CashierTaskData task = new CashierTaskData (1);
		var dialogues = task.Dialogues;
		//initialize inference dialogues, should be done in serialized files
		dialogues.Add(createLine("Hi", "Normal", "Morning", "Evening"));
		dialogues.Add(createLine("Hello", "Normal", "Morning", "Evening"));
		dialogues.Add(createLine( "Can I help you?", "Normal", "Morning", "Evening"));
		dialogues.Add(createLine("Good Morning", "Morning"));
		dialogues.Add(createLine("Good Evening", "Evening"));
		//initialize shop lists
		task.ShopLists.Add (createShopItem("item1", 1));
		task.ShopLists.Add (createShopItem("item2", 2));
		task.ShopLists.Add (createShopItem("item3", 3));
		task.ShopLists.Add (createShopItem("item4", 4));
		task.ShopLists.Add (createShopItem("item5", 5));
		task.ShopLists.Add (createShopItem("item6", 6));
		//initialze other parameters
		task.AreaName = "CashierTest";
		task.Name = "CashierTask";
		task.ProcessType = new ProcessTypeRef (typeof(CashierProcess));
		task.SceneObjectIdentifier.Name = "Customer";
		return task;
	}

	InferenceDialogueLine createLine(string text, params string[] category){
		InferenceDialogueLine line = new InferenceDialogueLine(new List<string>(category));
		line.Phrase = new PhraseSequence(text);
		return line;
	}

	ValuedItem createShopItem(string t, int i){
		ValuedItem item = ValuedItem.CreateInstance<ValuedItem>();
		item.Text = t;
		item.Value = i;
		return item;
	}
}
