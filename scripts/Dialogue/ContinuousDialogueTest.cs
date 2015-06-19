using UnityEngine;
using System.Collections;

public class ContinuousDialogueTest : MonoBehaviour {

    public string[] lines = new string[3];

    int lineNumber = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnMouseDown () {
        var line = lines[lineNumber];
        var p = new PhraseSequence(line);
        var dl = new DialogueActorLine();
        dl.Phrase = p;
        GetComponent<DialogueActor>().SetLine(dl);
        lineNumber = (lineNumber + 1) % lines.Length;
	}
}
