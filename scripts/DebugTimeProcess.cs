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
		//TODO switch jobs here
//		ret.Tasks.Add(CreateCashierTask());
//		ret.Tasks.Add(CreatePetFeederTask());
//		ret.Tasks.Add(CreatePointPlaceTask());
		ret.Tasks.Add(CreateVolunteerTask());
		return ret;
	}

	JobPlayerData CreatePlayerData(){
		var data = new JobPlayerData ();
		data.Unlocked = true;
		data.JobID = 10;
		return data;
	}


	//actually create an instance of the subclass of JobTaskGameData
	JobTaskGameData CreateVolunteerTask (){
		VolunteerTaskData task = new VolunteerTaskData ();
		//set dialogue
		var phrase = task.Dialogue.GetNewDialogueElement <LineDialogueElement>().Line.Phrase;
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "I feel"));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, "need"));
		//set question and answer data
//		task.AddQA("hungry", "restuarant");
//		task.AddQA("thirsty", "coffee shop");
//		task.AddQA ("tired", "hotel");
//		task.AddQA ("bored", "theatre");
		//initialize player dialogue
		var answerPhrase = task.AnswerDialogue.GetNewDialogueElement<LineDialogueElement> ().Line.Phrase;
		answerPhrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "You should go to"));
		answerPhrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, "answer"));
		//other initialization
		task.AreaName = "VolunteerTest";
		task.Name = "VolunteerTask";
		task.ProcessType = new ProcessTypeRef (typeof(VolunteerProcess));
		task.Actor.Name = "Asker";
		task.PlayerIdentifier.Name = "Player";
		return task;
	}
	JobTaskGameData CreatePointPlaceTask (){
		PointPlaceTaskData task = new PointPlaceTaskData ();
		//set dialogue
		var phrase = task.Dialogue.GetNewDialogueElement<LineDialogueElement> ().Line.Phrase;
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "I want to go to"));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, "place"));
		//set question and answer data
//		task.AddQA("restuarant", "restuarant", new Sprite());
//		task.AddQA("coffee shop", "coffee shop", new Sprite());
//		task.AddQA ("hotel", "hotel", new Sprite ());
//		task.AddQA ("theatre", "theatre", new Sprite ());
		//other initialization
		task.AreaName = "PointPlaceTest";
		task.Name = "PointPlaceTask";
		task.ProcessType = new ProcessTypeRef (typeof(PointPlaceProcess));
		task.Actor.Name = "Asker";
		return task;
	}
	JobTaskGameData CreatePetFeederTask ()
	{
		PetFeederTaskData task = new PetFeederTaskData ();
		//set dialogue
		var phrase = task.Dialogue.GetNewDialogueElement<LineDialogueElement> ().Line.Phrase;
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "The cat is"));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, "query"));
		//set question and answer data
//		task.AddQA("hungry", "fish", new Sprite());
//		task.AddQA("thristy", "milk", new Sprite());
//		task.AddQA ("tired", "bed", new Sprite ());
//		task.AddQA ("grumpy", "math", new Sprite ());
		//other initialization
		task.AreaName = "PetFeederTest";
		task.Name = "PetFeederTask";
		task.ProcessType = new ProcessTypeRef (typeof(PetFeederProcess));
		task.Actor.Name = "Pet";
		return task;
	}
	
	JobTaskGameData CreateCashierTask(){

		//task creation
		CashierTaskData task = new CashierTaskData (1);
		var dialogues = task.Dialogues;
		//initialize inference dialogues, should be done in serialized files
		dialogues.Add(createLine("Hi", "Normal", "Morning", "Evening"));
		dialogues.Add(createLine("Hello", "Normal", "Morning", "Evening"));
		dialogues.Add(createLine( "Can I help you?", "Normal", "Morning", "Evening"));
		dialogues.Add(createLine("Good Morning", "Morning"));
		dialogues.Add(createLine("Good Evening", "Evening"));
		//initialize shop lists
		task.ShopLists.Add (createValuedItem("item1", 1));
		task.ShopLists.Add (createValuedItem("item2", 2));
		task.ShopLists.Add (createValuedItem("item3", 3));
		task.ShopLists.Add (createValuedItem("item4", 4));
		task.ShopLists.Add (createValuedItem("item5", 5));
		task.ShopLists.Add (createValuedItem("item6", 6));
		//initialze other parameters
		task.AreaName = "CashierTest";
		task.Name = "CashierTask";
		task.ProcessType = new ProcessTypeRef (typeof(CashierProcess));
		task.Actor.Name = "Customer";
		return task;
	}

	//Convinience functions for task creation
	InferenceDialogueLine createLine(string text, params string[] category){
		InferenceDialogueLine line = new InferenceDialogueLine(new List<string>(category));
		line.Phrase = new PhraseSequence(text);
		return line;
	}

	ValuedItem createValuedItem(string t, int i){
		ValuedItem item = new ValuedItem();
		item.Text = new PhraseSequence(t);
		item.Value = i;
		return item;
	}
}
