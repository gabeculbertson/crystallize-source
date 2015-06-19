using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogueOverrideAudio : MonoBehaviour {

	public AudioClip clip;

	void Start(){
		
	}

	public AudioClip GetAudioClip(DialogueActorLine line){
		return clip;
	}

}
