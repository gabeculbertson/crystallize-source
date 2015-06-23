using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CashierProcess : IProcess<JobTaskRef, object> {

	public event ProcessExitCallback OnExit;

	GameObject person;
	JobTaskRef task;

	int remainingCount = 0;
	int correctCount = 0;
	float score = 0;

	ITemporaryUI<string, string> ui;

	int numItems = 3;
	int price;
	string greeting;
	ValuedItem[] nowItem;

	List<TextMenuItem> greetings;
	List<ValuedItem> prices;

	public void Initialize(JobTaskRef param1) {
		task = param1;
		remainingCount = GetTaskCount();
		person = new SceneObjectRef(task.Data.SceneObjectIdentifier).GetSceneObject();
		//get menu item lists
		greetings = new List<TextMenuItem> (GameObject.FindObjectsOfType<TextMenuItem> ());
		prices = new List<ValuedItem> (GameObject.FindObjectsOfType<ValuedItem> ());
		//Greeting Prompt
		ProcessLibrary.MessageBox.Get("Select the correct greeting.", HandleGreetingExit, this);
	}
	
	//Select Greeting
	void HandleGreetingExit(object sender, object obj) {
		GetNewGreeting();
		person.GetComponent<DialogueActor>().SetLine(task.Data.Line, GetNewGreetingContext());
		var ui = UILibrary.TextMenu.Get (greetings);
		ui.Complete += ui_GreetComplete;
	}

	void ui_GreetComplete(object sender, EventArgs<TextMenuItemEventArg> e) {
		//TODO cast e and get data

		//remainingCount--;
		if (e.Data.getName() == greeting) {
			correctCount++;
			var ui = UILibrary.PositiveFeedback.Get("");
			ui.Complete += Greet_Feedback_Complete;
		} else {
			var ui = UILibrary.NegativeFeedback.Get("");
			ui.Complete += Greet_Feedback_Complete;
		}
	}

	void Greet_Feedback_Complete(object sender, EventArgs<object> e) {
		ProcessLibrary.MessageBox.Get("Select the correct price.", HandleMessageBoxExit, this);
	}

	void HandleMessageBoxExit(object sender, object obj) {
		GetNewTargetPrice();
		person.GetComponent<DialogueActor>().SetLine(task.Data.Line, GetNewPriceContext());
		var ui = UILibrary.ValuedMenu.Get (prices);
		ui.Complete += ui_Complete;
	}
	
	void ui_Complete(object sender, EventArgs<ValuedItemEventArg> e) {
		//TODO cast e and compare
		remainingCount--;
		if (e.Data.getValue() == price) {
			correctCount++;
			var ui = UILibrary.PositiveFeedback.Get("");
			ui.Complete += Feedback_Complete;
		} else {
			var ui = UILibrary.NegativeFeedback.Get("");
			ui.Complete += Feedback_Complete;
		}
	}
	
	void Feedback_Complete(object sender, EventArgs<object> e) {
		if (remainingCount <= 0) {
			PlayExitDialogue();
		} else {
			ProcessLibrary.MessageBox.Get("Select the correct greeting.", HandleGreetingExit, this);
		}
	}
	
	void PlayExitDialogue() {
		var s = "";
		score = (float)correctCount / (GetTaskCount() * 2);
		if (score >= 0.99f) {
			s = "Amazing! Nice work.";
		} else if( score >= 0.75f){
			s = "Hm... Not bad.";
		} else if(score >= 0.25f){
			s = "You need to work harder.";
		} else {
			s = "What are you doing!? Keep this up and I'll fire you.";
		}
		
		var d = new DialogueSequence();
		var de = d.GetNewDialogueElement();
		de.Line = new DialogueActorLine();
		de.Line.Phrase = new PhraseSequence(s);
		ProcessLibrary.Conversation.Get(new ConversationArgs(person, d), HandleExitConversationExit, this);
	}
	
	void HandleExitConversationExit(object sender, object obj) {
		int money = (int)(10000 * score);
		PlayerDataConnector.AddMoney(money);
		string moneyString = string.Format("You made {0} yen today.", money);
		var ui = UILibrary.MessageBox.Get(moneyString);
		ui.Complete += ui_Complete;
	}
	
	void ui_Complete(object sender, EventArgs<object> e) {
		Exit();
	}
	
	public void ForceExit() {
		Exit();
	}
	
	void Exit() {
		if (ui != null) {
			ui.Close();
		}
		
		OnExit.Raise(this, null);
	}
	
	void GetNewTargetPrice(){
		nowItem = new ValuedItem[numItems];
		var priceArray = prices.ToArray ();
		for (int i = 0; i < numItems; i++) {
			nowItem[i] = priceArray[UnityEngine.Random.Range(0, priceArray.Length)];
		}
		price = 0;
		foreach (var item in nowItem) {
			price += item.Value;
		}
	}

	void GetNewGreeting(){
		//TODO use task info to create a mapping
		TextMenuItem[] greetArray = greetings.ToArray ();
		greeting = greetArray [UnityEngine.Random.Range (0, greetArray.Length)].text;
	}
	
	ContextData GetNewPriceContext() {
		var c = new ContextData();
		//TODO How to make this more flexible/dynamic?
		Func<string, int, string> priceString = (x, y) => String.Format("{0} (Price: {1} yen)", x, y);
		c.UpdateElement("price1", new PhraseSequence(priceString(nowItem[0].Text, nowItem[0].Value)));
		c.UpdateElement("price2", new PhraseSequence(priceString(nowItem[1].Text, nowItem[1].Value)));
		c.UpdateElement("price3", new PhraseSequence(priceString(nowItem[2].Text, nowItem[2].Value)));
		return c;
		//TODO
	}

	ContextData GetNewGreetingContext() {
		var c = new ContextData();
//		c.UpdateElement("region", new PhraseSequence(price));
		return c;
		//TODO 
	}
	int GetTaskCount(){
		return 5;
	}
}
