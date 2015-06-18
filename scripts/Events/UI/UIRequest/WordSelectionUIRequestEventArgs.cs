using UnityEngine;
using System.Collections;

public class WordSelectionUIRequestEventArgs : UIRequestEventArgs {

    public IPhraseDropHandler Container { get; set; }

    public WordSelectionUIRequestEventArgs(GameObject menuParent, IPhraseDropHandler container) : base(menuParent) {
        this.Container = container;
    }

}
