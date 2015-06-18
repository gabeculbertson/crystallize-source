using UnityEngine;
using System.Collections;

public class TakeItemResponseBehaviorGameData : ResponseBehaviorGameData {

    public override void DoResponse(PhraseSequence playerPhrase, NPCActorLine npcPhrase, GameObject npcGameObject, ContextData playerContextData) {
        base.DoResponse(playerPhrase, npcPhrase, npcGameObject, playerContextData);

        var iids = PlayerManager.main.playerData.ItemInventory.ItemIDs;
        for (int i = 0; i < iids.Count; i++) {
            var iid = iids[i];
            iids[i] = 0;
            if (iid != 0) {
                Debug.Log("Item: " + iid);
                CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new ItemGivenEventArgs(npcGameObject.transform.GetWorldID(), iid));
            }
        }
    }

}
