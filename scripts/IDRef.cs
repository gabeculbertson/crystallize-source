using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class IDRef<T1, T2> {

    public int ID { get; protected set; }
	public abstract T1 GameDataInstance { get; set;}
	public abstract T2 PlayerDataInstance { get; set;}

    public bool IsNull {
        get {
            return ID == -1;
        }
    }

    public IDRef(int id) {
        ID = id;
    }

}
