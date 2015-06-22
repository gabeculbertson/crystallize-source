using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIPallet : MonoBehaviour {

    const string ResourcePath = "UI/GUIPallet";
    static GUIPallet _instance;
    public static GUIPallet Instance {
        get {
            if (!_instance) {
                _instance = GameObjectUtil.GetResourceInstance<GUIPallet>(ResourcePath);
            }
            return _instance;
        }
    }

	public Texture2D WhiteBackground { get; set; }
	public GUIStyle WordCardStyle { get; set; }
	public GUIStyle LabelStyle { get; set; }
	public GUIStyle ClearStyle { get; set; }

	public Font defaultFont;
	public Sprite lockedWordShape;
	public Sprite leftSpeechBubble;
	public Sprite rightSpeechBubble;
    public Sprite leftPhoneSpeechBubble;
    public Sprite rightPhoneSpeechBubble;

    public Color importantMessageColor = Color.white;
    public Color lightGray = new Color(0.8f, 0.8f, 0.8f);
    public Color darkGray = new Color(0.2f, 0.2f, 0.2f);
    public Color inactiveColor = Color.white;
    public Color successColor = Color.white;
    public Color failureColor = Color.white;

    public Color selfColor = Color.white;
    public Color otherColor = Color.white;

	public Color verbColor = Color.white;
	public Color nounColor = Color.white;
	public Color pronounColor = Color.white;
	public Color adverbColor = Color.white;
	public Color adjectiveColor = Color.white;
	public Color particleColor = Color.white;
	public Color questionWordColor = Color.white;
	public Color greetingColor = Color.white;

	public Color rumorPhraseColor = Color.white;
	public Color normalPhraseColor = Color.white;
	public Color contructedPhraseColor = Color.white;
	public Color inventoryBackgroundColor = Color.white;
	public Color constructorBackgroundColor = Color.white;
	public Color objectiveWordColor = Color.white;
	public Color confirmButtonColor = Color.white;
	public Color disabledButtonColor = Color.white;

	public Color[] levelColors;
	public Color[] stageColors;
	
	public Color[] rumorColors;
	public Texture2D textBubblePointer;

	int colorIndex = 0;
	Dictionary<string, Color> rumorColorDictionary = new Dictionary<string, Color>();

	Dictionary<int, GUIStyle> labelStyles = new Dictionary<int, GUIStyle>();

	// Use this for initialization
	void Awake () {
		WhiteBackground = new Texture2D (1, 1);
		WhiteBackground.SetPixel (0, 0, Color.white);
		WhiteBackground.Apply ();

		WordCardStyle = new GUIStyle ();
		WordCardStyle.fontSize = 50;
		WordCardStyle.normal.background = WhiteBackground;
		WordCardStyle.normal.textColor = new Color(0.2f, 0.2f, 0.2f);
		WordCardStyle.hover.background = WhiteBackground;
		WordCardStyle.hover.textColor = Color.black;
		WordCardStyle.padding = new RectOffset (16, 16, 8, 8);
		WordCardStyle.alignment = TextAnchor.MiddleCenter;
		WordCardStyle.wordWrap = true;

		ClearStyle = new GUIStyle ();
		ClearStyle.fontSize = 36;
		ClearStyle.alignment = TextAnchor.MiddleCenter;
		ClearStyle.normal.textColor = Color.black;
		ClearStyle.hover.background = new Texture2D (1, 1);
		ClearStyle.hover.background.SetPixel (0, 0, Color.white);
		ClearStyle.hover.background.Apply ();
		ClearStyle.normal.background = ClearStyle.hover.background;
		ClearStyle.hover.textColor = Color.black;
		ClearStyle.padding = new RectOffset (16, 16, 8, 8);

		LabelStyle = new GUIStyle (WordCardStyle);
		LabelStyle.fontSize = 36;
	}

	public Color GetColorForRumor(string rumor){
		if(!rumorColorDictionary.ContainsKey(rumor)){
			rumorColorDictionary[rumor] = rumorColors[colorIndex];
			colorIndex++;
		}
		return rumorColorDictionary [rumor];
	}

	public GUIStyle GetLabelStyle(int fontSize){
		if(!labelStyles.ContainsKey(fontSize)){
			labelStyles[fontSize] = new GUIStyle(LabelStyle);
			labelStyles[fontSize].fontSize = fontSize;
		}
		return labelStyles [fontSize];
	}

	public Color GetColorForWordCategory(PhraseCategory category){
		switch (category) {
		case PhraseCategory.Verb:
			return verbColor;
		case PhraseCategory.Noun:
			return nounColor;
		case PhraseCategory.Pronoun:
			return pronounColor;
		case PhraseCategory.Adjective:
			return adjectiveColor;
		case PhraseCategory.Adverb:
			return adverbColor;
		case PhraseCategory.Particle:
			return particleColor;
		case PhraseCategory.Greeting:
			return greetingColor;
		case PhraseCategory.Question:
			return questionWordColor;
		default:
			return Color.white;
		}
	}

	public PhraseCategory[] GetColoredCategories(){
		return new PhraseCategory[] {
			PhraseCategory.Greeting,
			PhraseCategory.Verb,
			PhraseCategory.Noun,
			PhraseCategory.Pronoun,
			PhraseCategory.Particle,
			PhraseCategory.Question,
			PhraseCategory.Adjective,
			PhraseCategory.Adverb
		};
	}

}
