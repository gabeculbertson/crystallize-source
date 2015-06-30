using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData{
	public class CashierSellGoods : StaticSerializedTaskGameData<JobTaskGameData> {

		protected override void PrepareGameData() {

			task = new CashierTaskData (2);
			var cashierTask = (CashierTaskData) task;
			var dialogues = cashierTask.Dialogues;
			//initialize inference dialogues, should be done in serialized files
			dialogues.Add(createLine("Hi", "Normal", "Morning", "Evening"));
			dialogues.Add(createLine("Hello", "Normal", "Morning", "Evening"));
			dialogues.Add(createLine( "Can I help you?", "Normal", "Morning", "Evening"));
			dialogues.Add(createLine("Good Morning", "Morning"));
			dialogues.Add(createLine("Good Evening", "Evening"));
			//initialize shop lists
			cashierTask.ShopLists.Add (createValuedItem("item1", 1));
			cashierTask.ShopLists.Add (createValuedItem("item2", 2));
			cashierTask.ShopLists.Add (createValuedItem("item3", 3));
			cashierTask.ShopLists.Add (createValuedItem("item4", 4));
			cashierTask.ShopLists.Add (createValuedItem("item5", 5));
			cashierTask.ShopLists.Add (createValuedItem("item6", 6));

			Initialize("Sell goods as cashier", "CashierTest", "Customer");
			SetProcess<CashierProcess>();
			AddDialogues<CashierDialogue01>();
			AddDialogues<CashierDialogue02>();
		}

		//Convinience functions for task creation
		InferenceDialogueLine createLine(string text, params string[] category){
			InferenceDialogueLine line = new InferenceDialogueLine(new List<string>(category));
			line.Phrase = new PhraseSequence(text);
			return line;
		}
		
		ValuedItem createValuedItem(string t, int i){
			ValuedItem item = ValuedItem.CreateInstance<ValuedItem>();
			item.Text = new PhraseSequence(t);
			item.Value = i;
			return item;
		}

		protected void AddDialogues<V>() where V : StaticSerializedDialogueGameData, new() {
			((CashierTaskData)task).AllDialogues.Add( new V().GetDialogue() );
		}
	}
}
