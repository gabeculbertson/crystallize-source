using UnityEngine;
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
public class QATaskGameData: JobTaskGameData {

	public class QALine {
		PhraseSequence question;
		string answer;
		public PhraseSequence Question {get{return question;}}
		public string Answer {get{return answer;}}

		public QALine(PhraseSequence q, string a){
			question = q;
			answer = a;
		}
	}


	protected List<QALine> QAlist;

	public QATaskGameData() : base(){
		QAlist = new List<QALine> ();
	}

	public void AddQA(PhraseSequence question, string answer){
		QALine newline = new QALine (question, answer);
		QAlist.Add (newline);
	}

	public IEnumerable<QALine> GetQAs(){
		return QAlist.ToList ();
	}
}
