using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crystallize;

public class ConversationClientPanelUI : MonoBehaviour {

	const float Padding = 10f;

	public GameObject clientPortraitPrefab;
	public ConversationObjectivePanelUI objectivePanel;

	public List<ConversationClientData> clients = new List<ConversationClientData>();

	public List<ConversationClientUI> conversationClients = new List<ConversationClientUI>();

	public RectTransform rectTransform { get; private set; }

	//List<PhraseSegmentData> currentPhraseData;

	bool initialized = false;

	void OnEnable(){
		GetComponent<CanvasGroup> ().alpha = 0;
	}

	// Use this for initialization
	void Start () {
		foreach (var client in clients) {
			AddClient(client);
		}

		rectTransform = GetComponent<RectTransform> ();

		//ResetObjectives ();

		if (InteractiveDialogManager.main) {
			CrystallizeEventManager.UI.OnInteractiveDialogueOpened += HandleOnDialogOpened;
			InteractiveDialogManager.main.OnDialogSuccess += HandleOnDialogSuccess;
		}

		CrystallizeEventManager.main.OnLoad += HandleOnLoad;

		if (!initialized) {
			Load();
		}
	}

	void HandleOnLoad (object sender, System.EventArgs e)
	{
		Load ();
	}

	void HandleOnDialogOpened (object sender, System.EventArgs e)
	{
		var actor = sender as InteractiveDialogActor;
		SetClient (actor.GetComponent<ConversationClient> ().clientData);
	}

	void HandleOnDialogSuccess (object sender, System.EventArgs e)
	{
		PlayerManager.main.playerData.FriendData.SetFriendState (conversationClients [0].clientData.ID, 1);
		UnlockWords ();
	}

	// Update is called once per frame
	void Update () {
		GetComponent<CanvasGroup> ().alpha = Mathf.MoveTowards (GetComponent<CanvasGroup> ().alpha, 1f, 2f * Time.deltaTime);

		if (conversationClients.Count == 0) {
			return;
		}

		if(Input.GetKeyDown(KeyCode.Period)){
			ScrollListDown();
		}

		if(Input.GetKeyDown(KeyCode.Comma)){
			ScrollListUp();
		}

		/*if (currentPhraseData != conversationClients [0].PhraseData) {
			ResetObjectives();
		}*/

		var pos = (Vector2)rectTransform.position + new Vector2(Padding, Padding);
		var index = conversationClients.Count - 1;
		foreach (var client in conversationClients) {
			client.Anchor = pos;
			if(client == conversationClients[0]){
				client.TargetScale = 2f;
			} else {
				client.TargetScale = 1f;
			}
			client.rectTransform.SetSiblingIndex(index);
			index--;

			if(client.NeedsSet){
				client.Set();
			}

			//Debug.Log(ConversationClientUI.DefaultSize * client.TargetScale);
			pos.y += ConversationClientUI.DefaultSize * client.TargetScale + Padding;
		}

		/*if (!objectivePanel.Unlocked) {
			if(objectivePanel.IsComplete){
				UnlockWords();
			}
		}*/
	}

	public void SetClient(ConversationClientData clientData){
		var clientUI = (from c in conversationClients where c.clientData == clientData select c).FirstOrDefault ();
		if (!clientUI) {
			AddClient(clientData);
			clientUI = conversationClients[conversationClients.Count - 1];
		}
		SwitchToClient (clientUI);
	}

	public void AddClient(ConversationClientData clientData){
		PlayerManager.main.playerData.FriendData.SetFriendState (clientData.ID, 
		                                                         PlayerManager.main.playerData.FriendData.GetFriendData(clientData.ID).FriendLevel);

		var go = Instantiate (clientPortraitPrefab) as GameObject;
		go.GetComponent<ConversationClientUI> ().Initialize (clientData);
		go.GetComponent<ConversationClientUI> ().OnWordsChanged += HandleOnWordsChanged;
		go.transform.SetParent (transform);
		conversationClients.Add (go.GetComponent<ConversationClientUI> ());
	}

	void HandleOnWordsChanged (object sender, System.EventArgs e)
	{
		//ResetObjectives ();
	}

	public void SwitchToClient(ConversationClientUI client){
		var index = conversationClients.IndexOf (client);
		if (index == -1) {
			return;
		}

		if (!gameObject.activeSelf) {
			return;
		}

		if (index > conversationClients.Count / 2) {
			StartCoroutine (SwitchToClientSequence (client, true));
		} else {
			StartCoroutine (SwitchToClientSequence (client, false));
		}

		ResetObjectives ();
	}

	IEnumerator SwitchToClientSequence(ConversationClientUI client, bool isUp){
		while (conversationClients[0] != client) {
			if(isUp){
				ScrollListUp();
			} else {
				ScrollListDown();
			}

			yield return new WaitForSeconds(0.1f);	
		}
	}

	void ScrollListDown(){
		var first = conversationClients [0];
		first.NeedsSet = true;
		conversationClients.RemoveAt (0);
		conversationClients.Add (first);

		ResetObjectives ();
	}

	void ScrollListUp(){
		var obj = conversationClients [conversationClients.Count - 1];
		obj.NeedsSet = true;
		conversationClients.RemoveAt (conversationClients.Count - 1);
		conversationClients.Insert(0, obj);

		ResetObjectives ();
	}

	void ResetObjectives(){
		if (objectivePanel) {
			objectivePanel.Unlocked = false;
			if(conversationClients.Count > 0){
				objectivePanel.Set(conversationClients[0].PhraseData, 
				                   PlayerManager.main.playerData.FriendData.GetFriendData(conversationClients[0].clientData.ID).FriendLevel == 1);
				//currentPhraseData = conversationClients[0].PhraseData;
			} else {
				var l = new List<PhraseSegmentData>();
				objectivePanel.Set(l, false);
				//currentPhraseData = l;	
			}
		}
	}

	public void UnlockWords(){
		objectivePanel.UnlockWords ();
	}

	void Load(){
		foreach(var c in PlayerManager.main.playerData.FriendData.Friends){
			AddClient(ScriptableObjectDictionaries.main.clientDictionary.GetClientForID(c.ID));
		}
		initialized = true;
	}

}
