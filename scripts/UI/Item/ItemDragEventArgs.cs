using UnityEngine;
using System.Collections;

public class ItemDragEventArgs : System.EventArgs {

    public int ItemID { get; set; }

    public ItemDragEventArgs(int itemID) {
        ItemID = itemID;
    }

}
