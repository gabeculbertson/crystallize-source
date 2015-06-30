using UnityEngine;
using System.Collections;


namespace CrystallizeData{
	public class VolunteerDialogue01 : StaticSerializedDialogueGameData {
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			AddActor("People");
			AddLine ("ask for help to find place");
		}
		#endregion
		
	}
}