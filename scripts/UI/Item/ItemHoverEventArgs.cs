using UnityEngine;
using System.Collections;

public class ItemHoverEventArgs : System.EventArgs {

    public bool IsHovering { get; set; }
    public string Text { get; set; }
    public Vector2 Position { get; set; }

    public ItemHoverEventArgs(bool isHovering, string text, Vector2 position) {
        IsHovering = isHovering;
        Text = text;
        Position = position;
    }

}
