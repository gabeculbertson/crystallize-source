using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TimePlayerData {

    public static string GetSessionString(int session) {
        if(Enum.IsDefined(typeof(TimeSessionType), session)){
            return ((TimeSessionType)session).ToString();
        }
        return "UNDEFINED";
    }

    public int Day { get; set; }
    public int Session { get; set; }

    public TimePlayerData() { }

    public string GetFormattedString() {
        return string.Format("Day {0}: {1}", Day, GetSessionString(Session));
    }

}
