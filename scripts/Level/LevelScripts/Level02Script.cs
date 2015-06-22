using UnityEngine;
using System.Collections;

public class Level02Script : LevelScript {

    public Transform overhearTarget;
    public InteractiveDialogActor firstActor;
    public PhraseSegmentData thisPhrase;

    // Use this for initialization
    IEnumerator Start() {
        while (!LevelSystemConstructor.main) {
            yield return null;
        }

        PassiveDialogManager.main.OnDialogOpened += HandleOnPassiveDialogOpened;
        InteractiveDialogManager.main.OnDialogSuccess += HandleOnDialogSuccess;

        CrystallizeEventManager.UI.OnInteractiveDialogueOpened += HandleOnInteractiveDialogOpened;
        CrystallizeEventManager.PlayerState.OnWordFound += HandleOnWordFound;
    }

    void HandleOnWordFound(object sender, System.EventArgs e) {
        if (PlayerData.Instance.WordStorage.ContainsFoundWord(thisPhrase)) {
            TutorialCanvas.main.ClearLines();
        }
    }

    void HandleOnDialogSuccess(object sender, System.EventArgs e) {
        TutorialCanvas.main.ClearLines();
    }

    void HandleOnInteractiveDialogOpened(object sender, System.EventArgs e) {
        var a = sender as InteractiveDialogActor;
        if (a.AllObjectivesComplete) {
            MainCanvas.main.OpenNotificationPanel("Some words are given for free. You can learn about these later.");
        }

        if (!PlayerData.Instance.WordStorage.ContainsFoundWord(thisPhrase)) {
            MainCanvas.main.OpenNotificationPanel("You can find 'this' in an overheard dialogue.");
            TutorialCanvas.main.ClearLines();
            TutorialCanvas.main.CreateWorldLine(PlayerManager.Instance.PlayerGameObject.transform, overhearTarget);
        }
    }

    void HandleOnPassiveDialogOpened(object sender, System.EventArgs e) {
        MainCanvas.main.OpenNotificationPanel("You can overhear dialogues by approaching people. You will need to overhear dialogs to complete your objectives.");
    }

}
