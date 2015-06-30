using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class PointPlace : StaticSerializedTaskGameData<JobTaskGameData> {
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			task = new PointPlaceTaskData ();
			PointPlaceTaskData pointTask = (PointPlaceTaskData) task;
			//set dialogue
			SetDialogue<PointPlaceDialogue01>();
			//set question and answer data
			pointTask.AddQA("restuarant", "restuarant", new Sprite());
			pointTask.AddQA("coffee shop", "coffee shop", new Sprite());
			pointTask.AddQA ("hotel", "hotel", new Sprite ());
			pointTask.AddQA ("theatre", "theatre", new Sprite ());
			//other initialization
			Initialize("Point Place Task 1", "PointPlaceTest", "Asker");
			SetProcess<PointPlaceProcess>();
		}
		#endregion
		
	}
}