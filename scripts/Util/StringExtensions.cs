using UnityEngine;
using System.Collections;

public static class StringExtensions {

    public static string Truncate(this string s, int length) {
        if (s == null) {
            return s;
        }
        
        if (s.Length <= length) {
            return s;
        }
        return s.Substring(0, length);
    }

}