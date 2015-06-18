using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public class HorizontalConversationClientPanelUI : MonoBehaviour, IClientUI {

	public GameObject portraitPrefab;

	List<RectTransform> clientPortraits = new List<RectTransform>();

	public IEnumerable<RectTransform> Clients {
		get {
			return clientPortraits;
		}
	}

	// Use this for initialization
	IEnumerator Start () {
		while (!ObjectiveManager.main.Initialized) {
			yield return null;
		}

		TutorialCanvas.main.ClientUI = this;

		RefreshPanel ();
		CrystallizeEventManager.UI.OnProgressEvent += HandleOnProgressEvent;;
	}

	void HandleOnProgressEvent (object sender, System.EventArgs e)
	{
		RefreshPanel ();
	}

	public RectTransform GetEntry(ConversationClient client){
		foreach (var c in clientPortraits) {
			if(c.GetComponent<StaticClientPortraitUI>().Client == client){
				return c.GetComponent<RectTransform>();
			}
		}
		return null;
	}

	void RefreshPanel(){
		foreach (var clientPortrait in clientPortraits) {
			Destroy(clientPortrait.gameObject);
		}
		clientPortraits.Clear ();

		foreach (var actor in InteractiveDialogManager.main.transform.GetComponentsInChildren<InteractiveDialogActor>()) {
			if(actor is FlexibleInteractiveActor){
				continue;
			}

			if(actor.HasBeenVisited || !ObjectiveManager.main.UseQuesting){
				var instance = Instantiate(portraitPrefab) as GameObject;
				instance.transform.SetParent(transform);
				instance.GetComponent<StaticClientPortraitUI>().Initialize(actor);
				clientPortraits.Add(instance.GetComponent<RectTransform>());
			}
		}
	}

}
