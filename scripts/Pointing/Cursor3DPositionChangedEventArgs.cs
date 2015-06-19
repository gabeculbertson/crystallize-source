using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cursor3DPositionChangedEventArgs : System.EventArgs {

    public int PlayerID { get; set; }
    public SerializableVector3 Position { get; set; }

    public Cursor3DPositionChangedEventArgs(int playerID, Vector3 position) {
        PlayerID = playerID;
        Position = position;
    }

}
