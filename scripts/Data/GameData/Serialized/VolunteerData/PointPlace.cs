using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class PointPlace : StaticSerializedTaskGameData<PointPlaceTaskData> {
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			task = new PointPlaceTaskData ();
			//set dialogue
			SetDialogue<PointPlaceDialogue01>();
			//set question and answer data
			var restaurantPhrase = GetPhrase("restaurant");
			var coffeeShopPhrase = GetPhrase("coffee shop");
			var hotelPhrase = GetPhrase("hotel");
			var theatrePhrase = GetPhrase("theatre");
			task.AddQA(restaurantPhrase, "restaurant", new Sprite());
			task.AddQA(coffeeShopPhrase, "coffee shop", new Sprite());
			task.AddQA (hotelPhrase, "hotel", new Sprite ());
			task.AddQA (theatrePhrase, "theatre", new Sprite ());
			//other initialization
			Initialize("Point Place Task 1", "PointPlaceTest", "Asker");
			SetProcess<PointPlaceProcess>();
		}
		#endregion
		
	}
}