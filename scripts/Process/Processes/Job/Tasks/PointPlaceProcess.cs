using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class PointPlaceProcess : IProcess<JobTaskRef, object> {

	public event ProcessExitCallback OnExit;

	GameObject target;
	JobTaskRef task;
	PointPlaceTaskData taskData;
	
	int remainingCount = 0;
	int correctCount = 0;
	int totalTrials = 0;
	float score = 0;

	IEnumerable<QATaskGameData.QALine> qa;
	QATaskGameData.QALine currentQA;
	List<TextImageItem> menuOptions;

	public void ForceExit() {
		Exit();
	}
	
	void Exit() {
		OnExit.Raise(this, null);
	}

	public void Initialize (JobTaskRef param1)
	{
		task = param1;
		taskData = (PointPlaceTaskData)(param1.Data);

		target = new SceneObjectRef(taskData.SceneObjectIdentifier).GetSceneObject();
		remainingCount = GetTaskCount ();

		qa = taskData.GetQAs ();
		menuOptions = new List<TextImageItem> ();
		foreach (var line in qa) {
			TextImageItem item = new TextImageItem();
			//TODO disable text after getting image
//			item.showText = false;
			item.text = line.Answer;
			item.Image = taskData.getPicture(line.Answer);
			menuOptions.Add(item);
		}
		ProcessLibrary.MessageBox.Get("select the picture of the given place", StartTask, this);
	}

	void StartTask(object obj, object arg){
		//start by giving a random query
		getNewQuery ();
		StartQuestion();
	}

	void StartQuestion(){
		ProcessLibrary.Conversation.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext()), HandleQuestionExit, this);
	}
	
	void HandleQuestionExit(object sender, object arg){
		//provides a menu to select possible responses
		var ui = UILibrary.ImageTextMenu.Get (menuOptions);
		ui.Complete += HandleAnswerFeedBack;
	}

	void HandleAnswerFeedBack (object sender, EventArgs<TextMenuItemEventArg> e)
	{
		totalTrials++;
		if (e.Data.getName () == currentQA.Answer) {
			var ui = UILibrary.PositiveFeedback.Get("");
			correctCount++;
			ui.Complete += HandleFeedBackComplete;
		} 
		else {
			var ui = UILibrary.NegativeFeedback.Get("");
			ui.Complete += HandleRetry;
		}
	}

	void HandleRetry (object sender, EventArgs<object> e)
	{
		StartQuestion();
	}

	void HandleFeedBackComplete (object sender, EventArgs<object> e)
	{
		remainingCount--;
		if (remainingCount <= 0)
			PlayExitDialogue ();
		else
			StartTask (null, null);
	}

	void PlayExitDialogue ()
	{
		var s = "";
		score = (float)correctCount / totalTrials;
		if (score >= 0.99f) {
			s = "Thank you very much! You are so helpful.";
		} else if( score >= 0.75f){
			s = "Hm... thanks anyway.";
		} else if(score >= 0.25f){
			s = "You don't seem to know many places.";
		} else {
			s = "Don't just point to me random places!";
		}
		
		var d = new DialogueSequence();
		var de = d.GetNewDialogueElement();
		de.Line = new DialogueActorLine();
		de.Line.Phrase = new PhraseSequence(s);
		ProcessLibrary.Conversation.Get(new ConversationArgs(target, d), HandleExitConversationExit, this);
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

	void getNewQuery ()
	{
		currentQA = qa.ToArray()[UnityEngine.Random.Range (0, qa.Count ())];
	}

	ContextData getNewContext ()
	{
		ContextData c = new ContextData ();
		c.UpdateElement("place", new PhraseSequence(currentQA.Question));
		return c;
	}

	int GetTaskCount ()
	{
		return 3;
	}
}
