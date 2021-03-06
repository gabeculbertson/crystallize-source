﻿using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class PlacePointer : StaticSerializedJobGameData{
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			Initialize ("PlacePointer");
			AddTask<PointPlace>();
			job.TaskSelector = new VariationListSelectorGameData(2);

            job.Hide = false;
		}
		#endregion
	}
}