using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationPhrasePanelUI : MonoBehaviour {

    public GameObject phrasePrefab;

    DialogueSequenceEventArgs endArgs;
    DialogueSequenceEventArgs linearArgs;

    List<GameObject> phraseInstances = new List<GameObject>();

	// Use this for initialization
	void Start () {
        transform.SetParent(MainCanvas.main.transform);

	    CrystallizeEventManager.UI.OnPromptLinearDialogueContinue += HandlePromptLinearDialogueContinue;
        CrystallizeEventManager.UI.OnPromptEndDialogueContinue += HandlePromptEndDialogueContinue;
        CrystallizeEventManager.UI.OnPromptPromptDialogueContinue += HandlePromptPromptDialogueContinue;

        CrystallizeEventManager.PlayerState.OnSucceedCollectPhrase += HandleSucceedCollectPhrase;

        Refresh();
	}

    void Refresh()
    {
        UIUtil.GenerateChildren(PlayerData.Instance.PhraseStorage.Phrases, phraseInstances, transform, GetPhraseInstance);
    }

    GameObject GetPhraseInstance(PhraseSequence phrase)
    {
        var instance = Instantiate<GameObject>(phrasePrefab);
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
        instance.GetComponent<UIButton>().OnClicked += HandlePhraseClicked;
        return instance;
    }

    void HandlePhraseClicked(object sender, System.EventArgs e)
    {
        var c = (Component)sender;
        var p = c.gameObject.GetInterface<IPhraseContainer>().Phrase;
        CrystallizeEventManager.UI.RaiseBasePhraseSelected(this, new PhraseEventArgs(p));
    }

    void HandleSucceedCollectPhrase(object sender, PhraseEventArgs e)
    {
        Refresh();
    }

    void HandlePromptPromptDialogueContinue(object sender, DialogueSequenceEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    void HandlePromptEndDialogueContinue(object sender, DialogueSequenceEventArgs e)
    {
        //Debug.Log("end prompt");
        endArgs = e;
    }

    void HandlePromptLinearDialogueContinue (object sender, DialogueSequenceEventArgs args){
        //Debug.Log("linear prompt");
        linearArgs = args;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (!UISystem.MouseOverUI()) {
                if (linearArgs != null) {
                    CrystallizeEventManager.UI.RaiseResolveLinearDialogueContinue(this,
                        new DialogueSequenceEventArgs(linearArgs.Dialogue, linearArgs.Dialogue.GetElement(linearArgs.CurrentElement).NextIDs[0]));
                    linearArgs = null;
                    //Debug.Log("Linear continue");
                } else if (endArgs != null) {
                    CrystallizeEventManager.UI.RaiseResolveEndDialogueContinue(this,
                        new DialogueSequenceEventArgs(null, -1));
                    endArgs = null;
                    //Debug.Log("end continue");
                }
            }
        }
	}


}
