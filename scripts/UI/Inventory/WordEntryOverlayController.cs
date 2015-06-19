using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Crystallize;

public class WordEntryOverlayController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public GameObject overlayContainer;

	// Use this for initialization
	void Start () {
		overlayContainer.SetActive (false);	
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		//overlayContainer.SetActive (true);
	}
	
	public void OnPointerExit (PointerEventData eventData)
	{
		overlayContainer.SetActive (false);
	}

	public void PlayAudio(){
		var phrase = GetComponent<InventoryEntryUI> ().phraseData;
		//Debug.Log(
		if (phrase) {
			if(phrase.MaleAudioClip){
				GetAudio().clip = phrase.MaleAudioClip;
				GetAudio().time = 0;
				GetAudio().Play();
			}
		}
	}

	public void RemoveEntry(){
		Debug.Log ("Removing");
		var phrase = GetComponent<InventoryEntryUI> ().phraseData;
		if (phrase) {
			Destroy(gameObject);
		}
	}

	AudioSource GetAudio(){
		if (!GetComponent<AudioSource>()) {
			gameObject.AddComponent<AudioSource>();
		}
		return GetComponent<AudioSource>();
	}


}
