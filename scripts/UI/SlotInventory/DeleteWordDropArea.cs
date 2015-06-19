using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DeleteWordDropArea : UIMonoBehaviour, IPhraseDropHandler {

    public void AcceptDrop(IWordContainer phraseObject) {
        if (phraseObject.gameObject) {
            Destroy(phraseObject.gameObject);
        }
        var invEles = PlayerManager.main.playerData.WordStorage.InventoryElements;
        var i = invEles.IndexOf(phraseObject.Word);
        if (i >= 0) {
            invEles[i] = null;
        }
        CrystallizeEventManager.UI.RaiseUpdateUI(this, System.EventArgs.Empty);
    }
}
