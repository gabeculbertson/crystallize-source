using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Crystallize;

public class ConversationPhraseUI : MonoBehaviour, IInitializable<PhraseSequence>, IPhraseContainer {

    public PhraseSequence Phrase { get; set; }

    public void Initialize(PhraseSequence param1)
    {
        GetComponentInChildren<Text>().text = param1.GetText();
        Phrase = param1;
    }

}
