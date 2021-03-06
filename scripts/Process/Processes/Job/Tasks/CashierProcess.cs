﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class CashierProcess : IProcess<JobTaskRef, object> {

	public event ProcessExitCallback OnExit;

	GameObject person;
	JobTaskRef task;
	CashierTaskData taskData;

	int remainingCount = 0;
	int correctCount = 0;
	float score = 0;

	ITemporaryUI<string, string> ui;

	int numItems;
	int price;
	string greeting = "";
	ValuedItem[] nowItem;

	List<PhraseSequence> greetings;
	List<ValuedItem> prices;

	public void Initialize(JobTaskRef param1) {
		greetings = new List<PhraseSequence>();
		prices = new List<ValuedItem> ();

		task = param1;
		taskData = (CashierTaskData) (task.Data);
		remainingCount = GetTaskCount();
		numItems = taskData.NumItem;
		nowItem = new ValuedItem[numItems];
		person = new SceneObjectRef(taskData.Actor).GetSceneObject();
		//get menu item lists
		foreach (var v in taskData.Dialogues) {
			var item = v.Phrase;
			greetings.Add(item);
		}
		prices = taskData.ShopLists;
//		greetings = new List<TextMenuItem> (GameObject.FindObjectsOfType<TextMenuItem> ());
//		prices = new List<ValuedItem> (GameObject.FindObjectsOfType<ValuedItem> ());
		//Greeting Prompt
		ProcessLibrary.MessageBox.Get("Select the correct greeting.", HandleGreetingExit, this);
	}

	//Select Greeting
	void HandleGreetingExit(object sender, object obj) {
		GetNewGreeting();
		ContextData context = GetNewGreetingContext ();
		DialogueSequence d = taskData.AllDialogues[0];
		//ProcessLibrary.BeginConversation.Get
		//		ProcessLibrary.ConversationSegment
		var actor = new SceneObjectRef(d.Actors[0]).GetSceneObject();
		ProcessLibrary.BeginConversation.Get(new ConversationArgs(actor, d, context), HandleGreetingBegins, this);
//		person.GetComponent<DialogueActor>().SetLine(taskData.Line, GetNewGreetingContext());
	}

	void HandleGreetingBegins(object sender, object obj){
		ContextData context = GetNewGreetingContext ();
		DialogueSequence d = taskData.AllDialogues[0];
		//ProcessLibrary.BeginConversation.Get
		//		ProcessLibrary.ConversationSegment
		var actor = new SceneObjectRef(d.Actors[0]).GetSceneObject();
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(actor, d, context), HandleGreetingConversationExit, this);
	}

	void HandleGreetingConversationExit(object sender, object obj){
		var ui = UILibrary.PhraseSequenceMenu.Get (greetings);
		ui.Complete += ui_GreetComplete;
	}

	void ui_GreetComplete(object sender, EventArgs<PhraseSequence> e) {
		//TODO cast e and get data

		//remainingCount--;
		if (taskData.SameCategory(e.Data.GetText(), greeting)) {
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
		var dialogue = taskData.AllDialogues[1];
		var actor = new SceneObjectRef(dialogue.Actors[0]).GetSceneObject();
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(actor, dialogue, GetNewPriceContext()), EndConversation, this);
//		person.GetComponent<DialogueActor>().SetLine(taskData.priceLine, GetNewPriceContext());
//		var ui = UILibrary.ValuedMenu.Get (GetAllPrices());
//		ui.Complete += ui_Complete;
	}

	void EndConversation(object a , object b){
		var dialogue = taskData.AllDialogues[1];
		var actor = new SceneObjectRef(dialogue.Actors[0]).GetSceneObject();
		Debug.Log("finishing conversation");
		ProcessLibrary.EndConversation.Get(new ConversationArgs(actor, dialogue), HandlePriceConversationExit, this);
	}
	void HandlePriceConversationExit(object sender, object obj){
//		var ui = UILibrary.ValuedMenu.Get (GetAllPrices());
		var ui = UILibrary.NumberEntry.Get(this);
		ui.Complete += ui_Complete;
	}

	void ui_Complete(object sender, EventArgs<int> e) {
		//TODO cast e and compare
		remainingCount--;
		if (e.Data == price) {
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
		var de = d.GetNewDialogueElement<LineDialogueElement>();
		de.Line = new DialogueActorLine();
		de.Line.Phrase = new PhraseSequence(s);
		d.Actors.Add(taskData.Actor);
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
		PhraseSequence[] greetArray = greetings.ToArray ();
		greeting = greetArray [UnityEngine.Random.Range (0, greetArray.Length)].GetText();
	}
	
	ContextData GetNewPriceContext() {
		var c = new ContextData();
		//TODO How to make this more flexible/dynamic?

		PhraseSequence contextPhrase = new PhraseSequence();
		if(nowItem.Length > 0){
			var firstItem = nowItem[0];
			BuildPriceContextString(firstItem, contextPhrase);
			if(nowItem.Length > 1){
				for(int i = 1; i < nowItem.Length - 1; i++){
					contextPhrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.FixedWord, ","));
					BuildPriceContextString(nowItem[i], contextPhrase);
				}
				contextPhrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.FixedWord, ","));
				contextPhrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "and"));
				BuildPriceContextString(nowItem[nowItem.Length - 1], contextPhrase);
			}

		}
		c.UpdateElement("item", contextPhrase);
//		c.UpdateElement("price2", new PhraseSequence(priceString(nowItem[1].Text, nowItem[1].Value)));
//		c.UpdateElement("price3", new PhraseSequence(priceString(nowItem[2].Text, nowItem[2].Value)));
		return c;
		//TODO
	}

	void BuildPriceContextString(ValuedItem item, PhraseSequence phrase){
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, item.Text.GetText()));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "("));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "price"));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, ":"));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, item.Value.ToString()));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "yen"));
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, ")"));
	}

	ContextData GetNewGreetingContext() {
		var c = new ContextData();
		c.UpdateElement("greeting", new PhraseSequence(greeting));
		return c;
	}
	int GetTaskCount(){
		return 5;
	}

	List<ValuedItem> GetAllPrices(){
		List<ValuedItem> ret = new List<ValuedItem> ();
		int maxPrice = prices.Aggregate<ValuedItem, int> (0, (acc, el) => acc + el.Value);
		Func<int, ValuedItem> createPriceOption = 
			(k => {var item = new ValuedItem(); item.Value = k; item.Text = new PhraseSequence(k.ToString()); item.ShowValue = false; return item;});
		for (int i = 0; i < maxPrice; i++) {
			ret.Add(createPriceOption(i));
		}
		return ret;
	}
}
