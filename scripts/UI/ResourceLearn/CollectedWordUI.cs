using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectedWordUI : MonoBehaviour {

    public Text translationText;
    public Text wordText;
    public Image wordImage;

    public void Initialize(PhraseSequenceElement word) {
        translationText.text = word.GetTranslation();
        wordText.text = word.GetPlayerText();

        Color c = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());
        c = Color.Lerp(c, Color.white, 0.5f);
        wordImage.color = c;
    }

}