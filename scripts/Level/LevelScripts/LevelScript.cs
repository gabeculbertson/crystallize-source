using UnityEngine;
using System;
using System.Collections;

public class LevelScript : MonoBehaviour {

	bool waiting = false;

	public GameObject PlayerGameObject {
		get {
            return PlayerManager.Instance.PlayerGameObject;
		}
	}

	protected IEnumerator WaitForEvent(){
		waiting = true;
		
		while (waiting) {
			yield return null;
		}
	}

    protected void Continue() {
        waiting = false;
    }
	
	protected void Continue (object sender, EventArgs e)
	{
		Continue();
	}

    protected void SetMessage(string message, float duration = 0) {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new HelpMessageUIRequestEventArgs(null, message));
        //EffectManager.main.PlayMessage(message, GUIPallet.main.importantMessageColor, duration);
    }

    protected void ClearMessages() {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new HelpMessageUIRequestEventArgs(null, ""));
        //EffectManager.main.ClearMessages();
    }

    protected IEnumerator RunStateMachine<T>(Func<T> getState, Action<T> setState, T exitState) {
        var oldState = default(T);
        while (true) {
            var newState = getState();
            if (newState.Equals(exitState)) {
                break;
            }

            if (!newState.Equals(oldState)) {
                setState(newState);
                oldState = newState;
            }

            yield return null;
        }
    }

    protected QuestInstanceData GetQuestInstance(Transform questClient) {
        var q = GameData.Instance.QuestData.GetQuestInfoFromWorldID(questClient.GetWorldID());
        return PlayerData.Instance.QuestData.GetOrCreateQuestInstance(q.QuestID);
    }

}
