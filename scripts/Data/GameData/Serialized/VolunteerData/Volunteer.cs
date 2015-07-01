using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class Volunteer : StaticSerializedJobGameData {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			Initialize("Volunteer");
			AddTask<VolunteerHelpFindPlace>();
			job.TaskSelector = new VariationListSelectorGameData(2);
		}

		#endregion


	}
}