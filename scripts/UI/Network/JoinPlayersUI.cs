using UnityEngine;
using System.Collections;

public class JoinPlayersUI : UIMonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        //PlayerData.Instance.Flags.SetFlag(FlagPlayerData.PlayersSynced, true);
        if (PlayerData.Instance.Flags.GetFlag(FlagPlayerData.PlayersSynced)) {
            Destroy(gameObject);
            yield break;
        }

        yield return null;
        transform.SetParent(MainCanvas.main.transform);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        rectTransform.position = Vector3.zero;

        if (!PlayerData.Instance.Flags.GetFlag(FlagPlayerData.IsMultiplayer)) {
            Destroy(gameObject);
            yield break;
        }

        PlayerController.LockMovement(this);

        while (PlayerManager.Instance.OtherPlayerLevelID != Application.loadedLevel) {
            yield return new WaitForSeconds(0.1f);
        }

        PlayerController.UnlockMovement(this);
        PlayerData.Instance.Flags.SetFlag(FlagPlayerData.PlayersSynced, true);
        Destroy(gameObject);
	}
	
}
