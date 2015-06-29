using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class CashierDialogue01 : StaticSerializedDialogueGameData {
		protected override void PrepareGameData() {
			AddActor("Cashier");
			
			AddAnimation(new GestureDialogueAnimation("Bow"));
			AddLine("greeting");
			AddLine("I want to buy this item");
		}
	}
}
