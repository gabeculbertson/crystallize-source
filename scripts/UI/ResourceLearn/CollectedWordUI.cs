using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectedWordUI : MonoBehaviour {

    public Text translationText;
    public Text wordText;

    public void Initialize(PhraseSequenceElement word) {
        translationText.text = word.GetTranslation();
        wordText.text = word.GetPlayerText();
    }

}