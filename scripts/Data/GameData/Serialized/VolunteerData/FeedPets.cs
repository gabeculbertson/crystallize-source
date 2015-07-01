using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class FeedPets : StaticSerializedTaskGameData<JobTaskGameData> {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			task = new PetFeederTaskData();
			var feederTask = (PetFeederTaskData) task;
			var hungryPhrase = GetPhrase("hungry");
			var thirstyPhrase = GetPhrase("thirsty");
			var tiredPhrase = GetPhrase("tired");

			//set question and answer data
			feederTask.AddQA(hungryPhrase, new PhraseSequence("fish"), new Sprite());
			feederTask.AddQA(thirstyPhrase, new PhraseSequence("milk"), new Sprite());
			feederTask.AddQA (tiredPhrase, new PhraseSequence("bed"), new Sprite ());

			//other initialization
			Initialize("PetFeederTask", "PetFeederTest", "PetOwner");
			SetProcess<PetFeederProcess>();
			SetDialogue<PetFeederDialogue01>();
		}

		#endregion


	}
}