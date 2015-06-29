using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class CashierDialogue01 : StaticSerializedDialogueGameData {
		protected override void PrepareGameData() {
			AddActor("Cashier");
			AddActor("Customer");
			
			AddAnimation(0, new GestureDialogueAnimation("Bow"));
			AddLine(1, "greeting");
			AddLine(1, "I want to buy this item");
		}
	}
}
