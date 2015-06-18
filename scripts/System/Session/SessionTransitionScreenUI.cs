using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SessionTransitionScreenUI : MonoBehaviour, ILockEvent, IInitializable<string> {

    public Text nextDayText;

    float time = 3f;

    public event System.EventHandler OnUnlock;

    public void Initialize(string sessionLabel)
    {
        nextDayText.text = sessionLabel;
        transform.SetParent(MainCanvas.main.transform, false);
    }
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Unlock();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Unlock();
        }
	}

    void Unlock()
    {
        if (OnUnlock != null)
        {
            OnUnlock(this, System.EventArgs.Empty);
        }

        OnUnlock = null;
        Destroy(gameObject);
    }

}
