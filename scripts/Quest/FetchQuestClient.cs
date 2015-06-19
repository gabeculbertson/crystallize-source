using UnityEngine;
using System.Collections;

public class FetchQuestClient : MonoBehaviour {

    public string desiredItem = "";
    public PhraseSegmentData thanksPhrase;

    bool complete = false;

    void Start() {
        GetComponent<PassiveDialogActor>().OnOpenDialog += HandleOnOpenDialog;
    }

    void HandleOnOpenDialog(object sender, PhraseEventArgs e) {
        if (PlayerManager.main.playerData.Item == desiredItem && !complete) {
            GetComponent<QuestClient>().CompleteQuest();
            GetComponent<PassiveDialogActor>().phrase = thanksPhrase;
            GetComponent<PassiveDialogActor>().PlayPhrase();
            PlayerManager.main.PlayerGameObject.GetComponent<ArmAnimationController>().StopHolding();

            complete = true;
        }
    }

}
