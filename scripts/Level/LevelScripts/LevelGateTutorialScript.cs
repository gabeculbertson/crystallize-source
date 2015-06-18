using UnityEngine;
using System.Collections;

public class LevelGateTutorialScript : LevelScript {
	
	public DialogueActor targetActor;
	
	// Use this for initialization
	IEnumerator Start () {
		CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;

		yield return null;

		SetMessage("Complete the conversation to continue.");
	}
	
	void Update(){
		if (PlayerData.Instance.InventoryState.Level >= 1) {
			var linearDialogue = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(targetActor.GetWorldID());
			PlayerData.Instance.Conversation.SetAvailable(linearDialogue.ID, true);
			//Debug.Log(DialogueSystemManager.main.InteractionTarget);
			if(DialogueSystemManager.main.InteractionTarget.gameObject == targetActor.gameObject){
				Debug.Log(DialogueSystemManager.main.InteractionTarget);
				StartCoroutine(ReapproachSequence());
			}
			enabled = false;
		}
	}

	IEnumerator ReapproachSequence(){
		CrystallizeEventManager.Environment.RaiseActorDeparted(targetActor, System.EventArgs.Empty);

		yield return new WaitForSeconds (0.1f);

		CrystallizeEventManager.Environment.RaiseActorApproached(targetActor, System.EventArgs.Empty);
	}

	void HandleActorApproached (object sender, System.EventArgs e)
	{
		if (sender as DialogueActor == targetActor) {
			var linearDialogue = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(targetActor.GetWorldID());
			if(linearDialogue != null){
				if(!PlayerData.Instance.Conversation.IsAvailable(linearDialogue.ID)){
					if(PlayerData.Instance.InventoryState.Level >= 1){
						
					} else {
						EffectManager.main.PlayMessage("Need a higher speaker level!");
						SetMessage("Click on words to rank them up and increase your speaker level.");
					}
				}
			}
		}
	}
	
	
	
}
