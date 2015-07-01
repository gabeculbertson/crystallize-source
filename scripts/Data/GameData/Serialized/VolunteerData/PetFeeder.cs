using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class PetFeeder : StaticSerializedJobGameData {
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			Initialize("PetFeeder");
			AddTask<FeedPets>();
			job.TaskSelector = new VariationListSelectorGameData(2);

            job.Hide = false;
		}
		#endregion
		
	}
}
