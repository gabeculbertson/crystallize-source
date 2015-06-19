using UnityEngine;
using System.Collections;

public interface IDaySession {

    event System.EventHandler OnComplete;

    void Begin();

}
