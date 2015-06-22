using UnityEngine; 
using System.Collections; 

public class PhraseSelectionProcess : IProcess<object, PhraseSequence> {

    public static readonly ProcessFactoryRef<PhraseSequence, PhraseSequence> RequestPhraseEditor = new ProcessFactoryRef<PhraseSequence,PhraseSequence>();

    IPanelItemSelector<PhraseSequence> panel;
    IProcess childProcess;

    public event ProcessExitCallback OnExit;

    public void Initialize(object data) {
        panel = ConversationPhrasePanelUI.GetInstance();
        panel.OnItemSelected += HandleItemSelected;
        panel.SetInteractive(true);
    }

    public void ForceExit() {
        Exit(null);
    }

    void Exit(PhraseSequence args) {
        panel.OnItemSelected -= HandleItemSelected;
        panel.SetInteractive(false);
        OnExit.Raise(this, args);
    }

    void HandleItemSelected(object sender, EventArgs<PhraseSequence> e) {
        childProcess.TryForceExit();
        childProcess = RequestPhraseEditor.Get(e.Data, HandlePhraseSelection, this);
    }

    void HandlePhraseSelection(object sender, PhraseSequence args) {
        if (args != null) {
            Exit(args);
        }
    }

}
