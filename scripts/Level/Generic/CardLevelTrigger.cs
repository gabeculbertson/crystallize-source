using UnityEngine;
using System.Collections;

public class CardLevelTrigger : MonoBehaviour {

	//public CardManager cardManager;
	//public GameObject obstacle;
	public GameObject victoryEffect;
	public AudioClip sound;

	bool played = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ObjectiveManager.main.IsComplete && !played) {
			//EffectManager.main.EnqueueEffect(PlayEffect, 0);
            PlayEffect();
		}
	}

	void PlayEffect(){
		if(!played){
			if(sound){
				if(!GetComponent<AudioSource>()){
					gameObject.AddComponent<AudioSource>();
				}
				GetComponent<AudioSource>().clip = sound;
				GetComponent<AudioSource>().Play();
			}
			played = true;
			
			if(victoryEffect){
                Instantiate(victoryEffect, PlayerManager.Instance.PlayerGameObject.transform.position, Quaternion.identity);
			}
		}
	}
}
