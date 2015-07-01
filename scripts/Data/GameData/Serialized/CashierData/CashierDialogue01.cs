using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class CashierDialogue01 : StaticSerializedDialogueGameData {
		protected override void PrepareGameData() {

//			AddActor("Cashier");
			AddActor("Customer");
			
			AddAnimation(new GestureDialogueAnimation("Bow"));
			AddLine("[greeting]");

		}
	}
}
