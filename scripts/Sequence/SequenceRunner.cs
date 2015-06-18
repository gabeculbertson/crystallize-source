using UnityEngine;
using Unity.Sequence;

public class SequenceRunner : MonoBehaviour {
    static SequenceRunner main;

    public static void Run() {
        if (main == null) {
            var go = new GameObject("SequencePlayer");
            go.AddComponent<SequenceRunner>();
        }
    }

    void Awake() {
        main = this;
    }

    void Update() {
        Sequence.PlayFrame();
    }

    void OnDestroy() {
        Sequence.ResetAll();
		main = null;
    }
}
