using UnityEngine;
using System.Collections;

public class GiveItemResponseBehaviorGameData : ResponseBehaviorGameData {

    public override void DoResponse(PhraseSequence playerPhrase, NPCActorLine npcLine, GameObject npcGameObject, ContextData playerContextData) {
        base.DoResponse(playerPhrase, npcLine, npcGameObject, playerContextData);

        var itemContext = playerContextData.GetElement("tag.item");
        Debug.Log("Item context is: " + itemContext);
        if (itemContext != null) {
            var word = DictionaryData.Instance.GetEntryFromID(itemContext.Data.PhraseElements[0].WordID);
            if (word.HasAuxiliaryData) {
                var wordItem = word.AuxiliaryData.ItemID;
                if (wordItem != 0) {
                    PlayerData.Instance.ItemInventory.AddItem(wordItem);
                    CrystallizeEventManager.UI.RaiseItemAcquired(this, new ItemDragEventArgs(wordItem));
                    EffectManager.main.PlayMessage("Got " + GameData.Instance.TradeData.Items.GetItem(wordItem).Name.GetText(JapaneseTools.JapaneseScriptType.Romaji) + "!");
                }
            }

            /*var inv = GameData.Instance.DialogueData.NPCInventories.GetItem(npcGameObject.transform.GetWorldID());
            if (inv != null) {
                
            }*/
        } 

        /*var wid = npcGameObject.transform.GetWorldID();
        var context = GameData.Instance.DialogueData.PersonContextData.GetItem(wid);
        PhraseSegmentData p = null;
        if (context != null) {
            p = Phrase.GetPhraseFromSequence(npcPhrase, context);
            var supplContext = npcPhrase.GetSuppliedContextData();
            if (supplContext.Count > 0) {
                CrystallizeEventManager.main.RaiseOnPlayerDataEvent(this, new ContextDataExpressedEventArgs(wid, supplContext[0]));
            }
        } else {
            p = Phrase.GetPhraseFromSequence(npcPhrase);
        }
        npcGameObject.GetComponent<DialogueActor>().SetPhrase(p);*/
    }

}
