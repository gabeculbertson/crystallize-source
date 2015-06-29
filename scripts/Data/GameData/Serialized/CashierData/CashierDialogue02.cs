using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class CashierDialogue02 : StaticSerializedDialogueGameData {
		protected override void PrepareGameData() {
//			AddActor("Cashier");
			AddActor("Customer");
			

			AddLine("I want this item");
		}
	}
}
