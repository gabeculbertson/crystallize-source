using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueConfirmationUI : MonoBehaviour {

    public Text confirmationText;

    AreaGameData area;
    float timer = 1f;

    public void Initialize(AreaGameData area) {
        transform.SetParent(MainCanvas.main.transform);
        this.area = area; 
        confirmationText.text = string.Format("Click the check to continue to the next area when you are ready.", area.AreaName);
    }

    public void Confirm() {
        AreaManager.TransitionToArea(area);
    }

    public void Close() {
        Destroy(gameObject);
    }

    IEnumerator Start() {
        yield return null;

        var rt = GetComponent<RectTransform>();
        var size = rt.rect.size;
        Debug.Log(size + "; " + rt.pivot + "; " + rt.rect);
        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f) - 0.5f * size + new Vector2(rt.pivot.x * size.x, rt.pivot.y * size.y);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            var target = new Vector2(8, Screen.height * 0.5f + 64f);
            transform.position = Vector2.MoveTowards(transform.position, target, 1000f * Time.deltaTime);
        }
    }

}
