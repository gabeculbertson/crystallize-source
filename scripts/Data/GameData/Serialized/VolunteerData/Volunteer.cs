﻿using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class Volunteer : StaticSerializedJobGameData {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			Initialize("Volunteer");
			AddTask<VolunteerHelpFindPlace>();
			job.Requirements.Add(new PreviousJobRequirementGameData("PetFeeder"));
			job.Requirements.Add(new PreviousJobRequirementGameData("PlacePointer"));
			job.AddRequirement(GetPhrase("hungry"));
			job.AddRequirement(GetPhrase("thirsty"));
			job.AddRequirement(GetPhrase("restaurant"));
			job.AddRequirement(GetPhrase("coffee shop"));
			job.TaskSelector = new VariationListSelectorGameData(2);
		}

		#endregion


	}
}