using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;

public class PictureQATaskData : QATaskGameData {
	
	Dictionary<string, Sprite> answerPictureDictionary;

	public PictureQATaskData() : base(){
		answerPictureDictionary = new Dictionary<string, Sprite> ();
	}

	new public void AddQA(string question, string answer){
		base.AddQA (question, answer);
		answerPictureDictionary.Add (answer, new Sprite ());
	}

	public void AddQA(string question, string answer, Sprite picture){
		QALine newline = new QALine (question, answer);
		QAlist.Add (newline);
		answerPictureDictionary.Add (answer, picture);
	}

	public Sprite getPicture(string answer){
		Sprite ret;
		answerPictureDictionary.TryGetValue (answer, out ret);
		return ret;
	}
}
