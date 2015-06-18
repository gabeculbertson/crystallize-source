using UnityEngine;
using System.Collections;

namespace Crystallize {
	public class ItemGiver : MonoBehaviour {

		public PhraseSegmentData hello;
		public PhraseSegmentData waterPhrase;
		public PhraseSegmentData waterResponse;
		public PhraseSegmentData coffeePhrase;
		public PhraseSegmentData coffeeResponse;
		public PhraseSegmentData unsureResponse;

		// Use this for initialization
		void Start () {
			GetComponent<FlexibleInteractiveActor>().OnReactToPhrase += HandleOnReactToPhrase;
		}

		void HandleOnReactToPhrase (object sender, PhraseEventArgs e)
		{
			if (PhraseSegmentData.IsEquivalent (e.PhraseData, hello)) {
				GetComponent<FlexibleInteractiveActor> ().SetPhrase (hello);
			} else if (PhraseSegmentData.IsEquivalent (e.PhraseData, waterPhrase)) {
				GiveItem ("WaterBottle");
				GetComponent<FlexibleInteractiveActor> ().SetPhrase (waterResponse);
			} else if (PhraseSegmentData.IsEquivalent (e.PhraseData, coffeePhrase)) {
				GiveItem ("CoffeeCup");
				GetComponent<FlexibleInteractiveActor> ().SetPhrase (coffeeResponse);
			} 
		}

		void GiveItem(string item){
            PlayerManager.main.PlayerGameObject.GetComponent<ArmAnimationController>().HoldItem(item);
		}

	}
}
