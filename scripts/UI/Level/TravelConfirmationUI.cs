using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TravelConfirmationUI : MonoBehaviour {

    public Text confirmationText;

    AreaGameData area;

    public void Initialize(AreaGameData area) {
        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        this.area = area;
        confirmationText.text = string.Format("Go to <color=yellow>{0}</color>?", area.AreaName);
    }

    public void Confirm() {
        AreaManager.TransitionToArea(area);
    }

    public void Close() {
        Destroy(gameObject);
    }

}
