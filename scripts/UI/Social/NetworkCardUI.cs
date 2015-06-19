using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkCardUI : MonoBehaviour, IAnchoredUIElement {

	const string Slash = "  /  ";
	const float MoveSpeed = 2000f;
	
	public SocialData socialData;
	public string dotString;
	public Image portraitImage;
	public Text nameText;
	public Text infoText;
	public Text statusText;

	Vector2 anchor;
	bool isAnchored = false;

	public Vector2 Anchor { 
		get {
			return anchor;
		} set {
			isAnchored = true;
			anchor = value;
		}
	}

	// Use this for initialization
	void Start () {
		portraitImage.sprite = socialData.portrait;
		nameText.text = socialData.surname + dotString + socialData.givenName;
		infoText.text = socialData.occupation + Slash + socialData.age + Slash + socialData.homeTown;
		statusText.text = dotString + socialData.status;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnchored) {
			transform.position = Vector2.MoveTowards(transform.position, Anchor, MoveSpeed * Time.deltaTime);
		}
	}

	public void BeginAddFriendSequence(){
		StartCoroutine (AddSequence ());
	}

	IEnumerator AddSequence(){
		bool wasOpen = FriendsPanelUI.main.isOpen;
		FriendsPanelUI.main.isOpen = true;
		
		while(!FriendsPanelUI.main.IsVisible){
			yield return null;
		}

		FriendsPanelUI.main.AddFriend (gameObject);
		PlayerManager.main.playerData.FriendData.SetFriendState (socialData.ID, 0);
		
		yield return new WaitForSeconds(3f);
		
		FriendsPanelUI.main.isOpen = wasOpen;
	}
}
