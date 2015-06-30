using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class FeedPets : StaticSerializedTaskGameData<JobTaskGameData> {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			task = new PetFeederTaskData();
			var feederTask = (PetFeederTaskData) task;
			//set question and answer data
			feederTask.AddQA("hungry", "fish", new Sprite());
			feederTask.AddQA("thristy", "milk", new Sprite());
			feederTask.AddQA ("tired", "bed", new Sprite ());
			feederTask.AddQA ("grumpy", "math", new Sprite ());
			//other initialization
			Initialize("PetFeederTask", "PetFeederTest", "PetOwner");
			SetProcess<PetFeederProcess>();
			SetDialogue<PetFeederDialogue01>();
		}

		#endregion


	}
}