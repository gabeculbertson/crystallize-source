using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;

/**
 * An Extension of JobTaskGameData that deals with tasks that have more than one possible answer
 * The answers are grouped into categories and answers in the same category have the same correctness
 */
public class PetFeederTaskData : QATaskGameData {
	
	Dictionary<string, Sprite> answerPictureDictionary;

	public PetFeederTaskData() : base(){
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
