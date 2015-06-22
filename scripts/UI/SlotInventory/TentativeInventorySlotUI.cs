using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TentativeInventorySlotUI : MonoBehaviour {

    public void Intialize(PhraseSequenceElement word) {
        GetComponent<Image>().color = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());
        GetComponentInChildren<Text>().text = word.GetPlayerText();
    }

}
