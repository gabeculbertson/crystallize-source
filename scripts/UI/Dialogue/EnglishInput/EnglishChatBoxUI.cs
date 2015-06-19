using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnglishChatBoxUI : MonoBehaviour {

	static string chatHistory = "";

	static EnglishChatBoxUI instance;

	public InputField inputField;
	public Text chatText;
	public RectTransform textBackground;
	public Scrollbar scrollbar;

	void Awake(){
		/*if (instance) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);
		instance = this;

		Debug.Log ("Creating chatbox");*/
	}

	void OnDestroy(){
		//Debug.Log ("Destroying chatbox" + transform.parent);
	}

	// Use this for initialization
	IEnumerator Start () {
        if (GameSettings.GetFlag(UIFlags.LockEnglishChat)) {
            Destroy(gameObject);
            yield break;
        }

		yield return null;

		//if (MainCanvas.main) {
		transform.SetParent (MainCanvas.main.transform);
		//transform.position = new Vector2 (-446f, 10f);
		//transform.localPosition = new Vector2 (-446f, transform.localPosition.y);
		//}

		CrystallizeEventManager.Network.OnEnglishLineInput += HandleEnglishLineInput;
		//CrystallizeEventManager.main.BeforeSceneChange += HandleBeforeSceneChange;
		//CrystallizeEventManager.OnInitialized += HandleEventManagerInitialized;

		chatText.text = chatHistory;
	}

	void HandleBeforeSceneChange (object sender, System.EventArgs e)
	{
		transform.SetParent(null);
	}

	void HandleEventManagerInitialized(object sender, System.EventArgs e){
		CrystallizeEventManager.Network.OnEnglishLineInput += HandleEnglishLineInput;
		CrystallizeEventManager.Environment.BeforeSceneChange += HandleBeforeSceneChange;

		transform.SetParent (MainCanvas.main.transform);
		transform.position = new Vector2 (-446f, 10f);
		transform.localPosition = new Vector2 (-446f, transform.localPosition.y);
	}

	void HandleEnglishLineInput (object sender, TextEventArgs e)
	{
        DataLogger.LogTimestampedData("Chat", e.Text);
		chatHistory += "\n" + e.Text;
		chatText.text = chatHistory;
	}
	
	// Update is called once per frame
	void Update () {
		if (inputField.isFocused) {
			PlayerController.LockMovement (this);
		} else {
			PlayerController.UnlockMovement (this);
		}

		var textRect = chatText.GetComponent<RectTransform> ();
		var size = textRect.rect.height;
		var backSize = textBackground.rect.height - 8f;
		var scrollDist = size - backSize;
		if (scrollDist > 0) {
			scrollbar.size = (size - scrollDist) / size;
			textRect.localPosition = -scrollbar.value * scrollDist * Vector2.up + new Vector2(8f, 4f);
		} else {
			scrollbar.size = 1f;
			textRect.localPosition = new Vector2(8f, 4f);
		}
	}

	void OnGUI(){
		if(inputField.isFocused && Input.GetKey(KeyCode.Return)) {
			Confirm();
		}
	}

	public void Confirm(){
		if (inputField.text != "") {
			string text = "Player " + (PlayerManager.main.PlayerID + 1) + ": " + inputField.text;
			CrystallizeEventManager.Network.RaiseEnglishLineInput (this, new TextEventArgs (text));
			inputField.text = "";
		}
	}

}
