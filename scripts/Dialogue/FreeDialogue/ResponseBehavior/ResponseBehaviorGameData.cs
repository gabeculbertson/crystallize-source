using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[XmlInclude(typeof(GiveItemResponseBehaviorGameData))]
[XmlInclude(typeof(TakeItemResponseBehaviorGameData))]
public class ResponseBehaviorGameData {

    public virtual void DoResponse(PhraseSequence playerPhrase, NPCActorLine npcLine, GameObject npcGameObject, ContextData playerContextData) {
        var wid = npcGameObject.transform.GetWorldID();
        var context = GameData.Instance.DialogueData.PersonContextData.GetItem(wid);
        if (context != null) {
            var supplContext = npcLine.Phrase.GetSuppliedContextData();
            if (supplContext.Count > 0) {
                CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new ContextDataExpressedEventArgs(wid, supplContext[0]));
            }
        }
        npcGameObject.GetComponent<DialogueActor>().SetPhrase(npcLine.Phrase.InsertContext(context));
    }

}
