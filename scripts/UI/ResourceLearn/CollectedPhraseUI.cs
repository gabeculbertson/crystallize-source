using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectedPhraseUI : MonoBehaviour {

    public Text translationText;
    public Text phraseText;

    public void Initialize(PhraseSequence phrase) {
        translationText.text = phrase.Translation;
        phraseText.text = phrase.GetText(JapaneseTools.JapaneseScriptType.Romaji);
    }

}