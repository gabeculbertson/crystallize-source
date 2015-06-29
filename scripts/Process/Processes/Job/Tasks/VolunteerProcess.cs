using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class VolunteerProcess : IProcess<JobTaskRef, object> {

	public event ProcessExitCallback OnExit;

	GameObject target;
	GameObject player;
	JobTaskRef task;
	VolunteerTaskData taskData;
	
	int remainingCount = 0;
	int correctCount = 0;
	int totalTrials = 0;
	float score = 0;

	IEnumerable<QATaskGameData.QALine> qa;
	QATaskGameData.QALine currentQA;
	List<TextMenuItem> menuOptions;
	string currentAnswer;

	public void ForceExit() {
		Exit();
	}
	
	void Exit() {
		OnExit.Raise(this, null);
	}

	public void Initialize (JobTaskRef param1)
	{
		task = param1;
		taskData = (VolunteerTaskData)(param1.Data);

		target = new SceneObjectRef(taskData.Actor).GetSceneObject();
		player = new SceneObjectRef(taskData.PlayerIdentifier).GetSceneObject();
		remainingCount = GetTaskCount ();

		qa = taskData.GetQAs ();
		menuOptions = new List<TextMenuItem> ();
		foreach (var line in qa) {
			TextMenuItem item = new TextMenuItem();
			//TODO disable text after getting image
//			item.showText = false;
			item.text = line.Answer;
			menuOptions.Add(item);
		}
		ProcessLibrary.MessageBox.Get("Point people to places according to their need", StartTask, this);
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
		var ui = UILibrary.TextMenu.Get (menuOptions);
		ui.Complete += TalkBackToAsker;
	}

	void TalkBackToAsker(object sender, EventArgs<TextMenuItem> e){
		currentAnswer = e.Data.text;
		ProcessLibrary.Conversation.Get(new ConversationArgs(player, taskData.AnswerDialogue, getAnswerContext(currentAnswer)), HandleAnswerFeedBack, this);
	}

	void HandleAnswerFeedBack (object sender, object e)
	{
		totalTrials++;
		if (currentAnswer == currentQA.Answer) {
			var ui = UILibrary.PositiveFeedback.Get("");
			correctCount++;
			ui.Complete += HandleFeedBackComplete;
		} 
		else {
			var ui = UILibrary.NegativeFeedback.Get("");
			ui.Complete += HandleFeedBackComplete;
		}
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
			s = "You did a good job.";
		} else if( score >= 0.75f){
			s = "You can improve yourself on this.";
		} else if(score >= 0.25f){
			s = "You did not do well.";
		} else {
			s = "You did terribly. Do better next time";
		}
		ProcessLibrary.MessageBox.Get(s,HandleExitConversationExit,this);
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
		c.UpdateElement("need", new PhraseSequence(currentQA.Question));
		return c;
	}

	ContextData getAnswerContext(string s)
	{
		ContextData c = new ContextData ();
		c.UpdateElement("answer", new PhraseSequence(s));
		return c;
	}

	int GetTaskCount ()
	{
		return 7;
	}
}
