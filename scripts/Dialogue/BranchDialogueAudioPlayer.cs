using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public class BranchDialogueAudioPlayer : MonoBehaviour {

    public List<AudioClip> clips = new List<AudioClip>();

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.UI.OnSpeechBubbleRequested += main_OnSpeechBubbleOpen;   
	}

    void main_OnSpeechBubbleOpen(object sender, SpeechBubbleRequestedEventArgs e) {
        if ((DialogueActor)sender != GetComponent<DialogueActor>()) {
            //Debug.Log(sender);
            return;
        }

        if (e.Phrase == null) {
            //Debug.Log(e.Phrase);
            return;
        }

        var gid = transform.GetWorldID();
        var bd = GameData.Instance.DialogueData.GetBranchedDialogueForWorldObject(gid);
        if (bd != null) {
            int count = 0;
            foreach (var b in bd.Elements) {
                if (b == DialogueSystemManager.main.branch){
                    //e.Phrase.FulfillsTemplate(b.ResponsePhrase)) {
                    PlayAudio(clips[count]);
                    break;
                }
                count++;
            }
        } else {
            //Debug.Log(gid);
        }
    }

    void PlayAudio(AudioClip clip) {
        if (!clip) {
            return;
        }

        if (!GetComponent<AudioSource>()) {
            gameObject.AddComponent<AudioSource>();
        }
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().time = 0;
        GetComponent<AudioSource>().Play();
    }
	
}
