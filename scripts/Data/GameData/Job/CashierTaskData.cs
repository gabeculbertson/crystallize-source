using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CashierTaskData : InferenceTaskGameData {
	//number of items a customer will buy
	public int NumItem { get; set;}

	public List<DialogueSequence> AllDialogues;

	public string contextPrefix = "price";

	public DialogueActorLine priceLine;

	public List<ValuedItem> ShopLists { get; set;}

	public CashierTaskData() : base() {
		NumItem = 0;
		priceLine = new DialogueActorLine ();
		ShopLists = new List<ValuedItem> ();
		AllDialogues = new List<DialogueSequence>(); 
	}
	public CashierTaskData(int num) : this (){
		NumItem = num;
		priceLine.Phrase = new PhraseSequence("I want to buy ");
		PhraseSequence phrase = priceLine.Phrase;
		for (int i = 0; i < NumItem; i++) {
			phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, contextPrefix+i.ToString()));
			phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, " "));
		}
		Line.Phrase = new PhraseSequence ();
		Line.Phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, "greeting"));
	}
}
