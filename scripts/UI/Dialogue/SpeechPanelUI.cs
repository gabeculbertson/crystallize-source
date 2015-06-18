﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpeechPanelUI : MonoBehaviour {

	const float Padding = 2f;

	public static SpeechPanelUI main { get; set; }

	public GameObject normalSpeechBubblePrefab;
	public GameObject playerSpeechBubblePrefab;

    Dictionary<Transform, GameObject> speechBubbleInstances = new Dictionary<Transform, GameObject>();

	void Awake(){
		main = this;
	}

    void Start() {
        CrystallizeEventManager.UI.OnSpeechBubbleRequested += HandleSpeechBubbleRequested;
    }

    void HandleSpeechBubbleRequested(object sender, SpeechBubbleRequestedEventArgs e) {
        if (e.Phrase == null) {
            RemoveSpeechBubble(e.Target);
        } else {
            GetSpeechBubble(e.Target, e.Phrase, e.SpeechBubblePointerType, e.CanEdit, e.CheckGrammar);
        }
    }

    public GameObject GetSpeechBubble(Transform target, PhraseSegmentData phraseData, PointerType pointerType = PointerType.Normal) {
        var go = Instantiate(normalSpeechBubblePrefab) as GameObject;
        go.transform.SetParent(transform);
        go.GetComponent<SpeechBubbleUI>().phrase = phraseData.GetPhraseSequence();
        go.GetComponent<SpeechBubbleUI>().target = target;
        go.GetComponent<SpeechBubbleUI>().PointerType = pointerType;
        speechBubbleInstances[target] = go;
        return go;
    }

    public GameObject GetSpeechBubble(Transform target, PhraseSequence phrase, PointerType pointerType, bool canEdit, bool checkGrammar) {
        var go = Instantiate(normalSpeechBubblePrefab) as GameObject;
        go.transform.SetParent(transform);
        go.GetComponent<SpeechBubbleUI>().Initialize(target, phrase, pointerType, canEdit, checkGrammar);

        if (speechBubbleInstances.ContainsKey(target)) {
            Destroy(speechBubbleInstances[target]);
        }
        speechBubbleInstances[target] = go;

        return go;
    }

    public void RemoveSpeechBubble(Transform target) {
        if (speechBubbleInstances.ContainsKey(target)) {
            Destroy(speechBubbleInstances[target]);
            speechBubbleInstances.Remove(target);
        }
    }

	void Update(){
		AdjustBubbles ();
	}

	void AdjustBubbles(){
		var bubbles = new HashSet<SpeechBubbleUI> (GetComponentsInChildren<SpeechBubbleUI>());
		if (bubbles.Count <= 1) {
			return;
		}

		var minBubble = bubbles.First ();
		foreach (var bubble in bubbles) {
			bubble.Flipped = true;
			if(bubble.RootPosition.x > minBubble.RootPosition.x){
				minBubble = bubble;
			}
		}
		minBubble.Flipped = true;

		//var sortedBubbles = new List<SpeechBubbleUI> ();
		SpeechBubbleUI previous = null;
		while(bubbles.Count > 0){
			var lowest = bubbles.First();
			foreach(var b in bubbles){
				if(b.rectTransform.position.y < lowest.rectTransform.position.y){
					lowest = b;
				}
			}
			bubbles.Remove(lowest);

			if(previous != null){
				var r1 = previous.rectTransform.rect;
				r1.position += previous.RootPosition + previous.FlipOffset;
				var r2 = lowest.rectTransform.rect;
				r2.position += lowest.RootPosition + lowest.FlipOffset;

				if(r1.Overlaps(r2)){
					var top = previous.rectTransform.position.y + previous.rectTransform.rect.height + Padding;
					var diff = top - lowest.RootPosition.y;
					lowest.TargetVerticalOffset = diff * Vector2.up;
				} else {
					lowest.TargetVerticalOffset = Vector2.zero;
				}
			} else {
				lowest.TargetVerticalOffset = Vector2.zero;
			}
			previous = lowest;
		}
	}

}
