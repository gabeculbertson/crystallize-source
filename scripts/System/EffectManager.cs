using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class EffectManager : MonoBehaviour {

	class EnqueuedEffect {
		public Action action;
		public float duration;

		public EnqueuedEffect(Action action, float duration){
			this.action = action;
			this.duration = duration;
		}
	}

	public static EffectManager main { get; private set; }

	float timer = 0;
	EnqueuedEffect currentEffect;
	Queue<EnqueuedEffect> enqueuedEffects = new Queue<EnqueuedEffect> ();

	void Awake(){
		main = this;
	}

	void Update(){
		if (currentEffect == null) {
			if(enqueuedEffects.Count > 0){
				currentEffect = enqueuedEffects.Dequeue();
				currentEffect.action();
				timer = currentEffect.duration;
			}
		} else {
			timer -= Time.deltaTime;
			if(timer <= 0){
				currentEffect = null;
			}
		}

        /*if (Input.GetKeyDown(KeyCode.O)) {
            PlayPositiveFeedback();
        }*/
	}

	public void CreateExperiencePointsEffect(RectTransform target, int amount){
		Debug.Log("Making xp effect.");
		var go = Instantiate<GameObject> (EffectLibrary.Instance.uiRisingText);
		go.transform.SetParent(MainCanvas.main.transform);
		go.transform.position = (Vector2)target.position + target.rect.center;
		go.GetComponent<UILevelUpEffect>().levelUpString = "+" + amount + " xp";
		go.GetComponent<UILevelUpEffect>().textColor = Color.white;
	}

	public void CreateRisingTextEffect(RectTransform target, string text){
		//Debug.Log("Making rising text effect.");
		var go = Instantiate<GameObject> (EffectLibrary.Instance.uiRisingText);
        go.transform.SetParent(MainCanvas.main.transform);
		go.transform.position = (Vector2)target.position + target.rect.center;
		go.GetComponent<UILevelUpEffect>().levelUpString = text;
		go.GetComponent<UILevelUpEffect>().textColor = Color.yellow;
	}

	public void CreateFlashEffect(RectTransform target){
        var flashEffect = Instantiate<GameObject>(EffectLibrary.Instance.uiSetEffect);
		flashEffect.transform.SetParent(target.transform);
	}

	public void EnqueueEffect(Action action, float duration){
		//Debug.Log ("Effect enqueued.");
		enqueuedEffects.Enqueue (new EnqueuedEffect (action, duration));
	}

	void PlayEnqueuedLargeTextEffect(string text, Color color, float duration){
        ClearMessages();
        
        //var effect = Instantiate (conversationCompleteEffect) as GameObject;
        var effect = Instantiate(EffectLibrary.Instance.uiMessage) as GameObject;
        effect.GetComponent<UIPlayerLevelUpEffect>().duration = duration;
		var tc = effect.GetComponentInChildren<Text> ();
		tc.text = text;
		if (color != default(Color)) {
			tc.color = color;
		}
		//effect.transform.SetParent (MainCanvas.main.transform);
        effect.transform.SetParent(MessagePanelUI.main.transform);
		//effect.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
	}

    public void PlayMessage(string text, Color color = default(Color), float duration = 3f) {
        EnqueueEffect(() => PlayEnqueuedLargeTextEffect(text, color, duration), 0.5f);
    }

    public void PlayPositiveFeedback() {
        Instantiate(EffectLibrary.Instance.positiveFeedbackEffect);
    }

    public void ClearMessages() {
        foreach (Transform t in MessagePanelUI.main.transform) {
            t.GetComponent<UIPlayerLevelUpEffect>().canFade = true;
        }
    }

}
