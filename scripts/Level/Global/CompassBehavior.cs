using UnityEngine;
using System.Collections;

public class CompassBehavior : MonoBehaviour {

	public Camera uiCamera;
	public Transform arrow;
    public Transform allyArrow;
    
    RectTransform positionRect;
	//public Transform target;

	// Use this for initialization
	IEnumerator Start () {
        if (GameSettings.GetFlag(UIFlags.LockCompass)) {
            Destroy(gameObject);
            yield break;
        }

        yield return null;
        positionRect = TutorialCanvas.main.GetRegisteredGameObject("CompassPlaceholder").GetComponent<RectTransform>();

		if (!LevelSettings.main.useQuesting) {
			gameObject.SetActive(false);
            yield break;
            //return;
		}

		CrystallizeEventManager.Environment.AfterCameraMove += HandleAfterCameraMove;

        transform.position = uiCamera.ScreenToWorldPoint(new Vector3(positionRect.position.x, positionRect.position.y, 10f));
        //uiCamera.ScreenToWorldPoint (new Vector3 (Screen.width * 0.5f - 530f, 69f, 10f));
	}

	void HandleAfterCameraMove (object sender, System.EventArgs e)
	{
		transform.rotation = Quaternion.Inverse (Camera.main.transform.rotation);
		if (QuestManager.main.HasActiveTarget()) {
			arrow.gameObject.SetActive(true);
            var playerPos = PlayerManager.main.PlayerGameObject.transform.position;
			var dir = QuestManager.main.GetActiveTarget() - playerPos;
			//Debug.Log("dir: " + dir + "; " + QuestManager.main.GetActiveTarget() + "; " + playerPos);
			dir.y = 0;
			arrow.localRotation = Quaternion.LookRotation (-dir.normalized);
		} else {
			arrow.gameObject.SetActive(false);
		}

        var ally = InteractionManager.AllyGameObject();
        if (ally) {
            allyArrow.gameObject.SetActive(true);
            var dir = ally.transform.position - PlayerManager.main.PlayerGameObject.transform.position;
            dir.y = 0;
            allyArrow.localRotation = Quaternion.LookRotation(-dir.normalized);
        } else {
            allyArrow.gameObject.SetActive(false);
        }
	}

}
