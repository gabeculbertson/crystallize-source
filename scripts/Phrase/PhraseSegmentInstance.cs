using UnityEngine;
using System.Collections;

/// <summary>
/// Phrase segment instance for storing positional and other transient information.
/// </summary>
namespace Crystallize {
	public class PhraseSegmentInstance {

		[SerializeField]
		PhraseSegmentData data;

		public PhraseSegmentData Data {
			get {
				return data;
			}
		}

		public string Text { 
			get {
				return data.Text;
			}
		}

		public GameObject DefaultPrefab {
			get {
				return data.Prefab;
			}
		}

		public bool IsHidden { get; set; }

		public Vector2 Center { get; set; }
		public Vector2 Anchor { get; set; }
		public float Width { get; set; }
		public Rect Rect { get; set; }

		public PhraseSegmentInstance(PhraseSegmentData data){
			this.data = data;
		}

		public bool Draw(){
			return Draw (GUIPallet.main.WordCardStyle);
		}

		public bool Draw(GUIStyle style, Vector2 anchor = default(Vector2)){
			var content = new GUIContent (data.Text);
			var size = style.CalcSize (content);
			var r = new Rect (0, 0, size.x, size.y);
			r.center = Center + anchor;
			Rect = r;
			Width = size.x;

			if (IsHidden) {
				GUI.color = new Color (1f, 1f, 1f, 0.5f);
			} else {
				GUI.color = Color.white;
			}

			//if (Data.IsRumorSegment) {
			//	GUI.backgroundColor = GUIPallet.main.rumorPhraseColor; //InteractiveDialogPanel.main.rumorPhraseColor;
			//} else if (Data.IsConstructed) {
			//	GUI.backgroundColor = GUIPallet.main.contructedPhraseColor; 
			//} else {
				GUI.backgroundColor = GUIPallet.main.normalPhraseColor; 
			//}

			return GUI.RepeatButton (Rect, content, style);
		}

	}
}