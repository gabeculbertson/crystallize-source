using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class PlacePointer : StaticSerializedJobGameData{
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			Initialize ("PlacePointer");
			AddTask<PointPlace>();

            job.Hide = true;
		}
		#endregion
	}
}