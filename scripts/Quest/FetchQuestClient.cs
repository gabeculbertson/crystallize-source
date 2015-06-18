using UnityEngine;
using System.Collections;

public class FetchQuestClient : MonoBehaviour {

	public string desiredItem = "";
	public PhraseSegmentData thanksPhrase;

	bool complete = false;

	void Start(){
		GetComponent<Crystallize.PassiveDialogActor>().OnOpenDialog += HandleOnOpenDialog;
	}

	void HandleOnOpenDialog (object sender, PhraseEventArgs e)
	{
		if (PlayerManager.main.playerData.Item == desiredItem && !complete) {
			GetComponent<QuestClient>().CompleteQuest();
			GetComponent<Crystallize.PassiveDialogActor>().phrase = thanksPhrase;
			GetComponent<Crystallize.PassiveDialogActor>().PlayPhrase();
            PlayerManager.main.PlayerGameObject.GetComponent<ArmAnimationController>().StopHolding();

			complete = true;
		} 
	}

}
