using UnityEngine;
using System.Collections;

public class PersistentSystem {

    static PersistentSystem _instance;

    static PersistentSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PersistentSystem();
            }
            return _instance;
        }
    }

}
