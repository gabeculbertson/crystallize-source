﻿using UnityEngine;
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
public class InferenceTaskGameData: JobTaskGameData {

	public List<InferenceDialogueLine> Dialogues { get; set;}

	public InferenceTaskGameData() : base(){
		Dialogues = new List<InferenceDialogueLine> ();
	}

	//TODO more useful data structures
	public bool SameCategory (string g1, string g2){
		PhraseSequence p1 = new PhraseSequence (g1);
		PhraseSequence p2 = new PhraseSequence (g2);
		List<string> l1 = new List<string> ();
		List<string> l2 = new List<string> ();
		foreach (var d in Dialogues){
//			if(PhraseSequence.IsPhraseEquivalent(p1, d.Phrase)){
			if(d.Phrase.GetText() == g1){
				l1 = new List<string>(d.Category);
			}
		}

		foreach (var d in Dialogues){
			if(d.Phrase.GetText() == g2){
				l2 = new List<string>(d.Category);
			}
		}
		return l1.Intersect (l2).ToList().Count > 0;
	}
}
