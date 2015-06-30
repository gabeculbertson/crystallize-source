using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UILibrary {

    public static readonly UIFactoryRef<string, object> MessageBox = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> SessionTransition = new UIFactoryRef<string,object>();
    public static readonly UIFactoryRef<PhraseSequence, PhraseSequence> PhraseSelector = new UIFactoryRef<PhraseSequence, PhraseSequence>();
    public static readonly UIFactoryRef<PhraseSequence, PhraseSequence> PhraseEditor = new UIFactoryRef<PhraseSequence, PhraseSequence>();
    public static readonly UIFactoryRef<PhraseSequenceElement, PhraseSequenceElement> WordSelector = new UIFactoryRef<PhraseSequenceElement, PhraseSequenceElement>();
    public static readonly UIFactoryRef<GameObject, object> ConversationCamera = new UIFactoryRef<GameObject, object>();
    public static readonly UIFactoryRef<string, string> ContextActionButton = new UIFactoryRef<string, string>();
    public static readonly UIFactoryRef<string, object> PositiveFeedback = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> NegativeFeedback = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<object, object> MoneyState = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, object> HomeShopPanel = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, HomeRef> HomeSelectionPanel = new UIFactoryRef<object, HomeRef>();
    public static readonly UIFactoryRef<object, object> SkipSessionButton = new UIFactoryRef<object, object>();
	public static readonly UIFactoryRef<List<TextMenuItem>, TextMenuItem> TextMenu = new UIFactoryRef<List<TextMenuItem>, TextMenuItem>();
	public static readonly UIFactoryRef<List<ValuedItem>, ValuedItem> ValuedMenu = new UIFactoryRef<List<ValuedItem>, ValuedItem>();
	public static readonly UIFactoryRef<List<TextImageItem>, TextImageItem> ImageTextMenu = new UIFactoryRef<List<TextImageItem>, TextImageItem>();
	public static readonly UIFactoryRef<List<PhraseSequence>, PhraseSequence> PhraseSequenceMenu = new UIFactoryRef<List<PhraseSequence>, PhraseSequence>();
    public static readonly UIFactoryRef<object, int> Review = new UIFactoryRef<object, int>();
    public static readonly UIFactoryRef<object, JobRef> Jobs = new UIFactoryRef<object, JobRef>();
	//UI for Cashier
//	public static readonly UIFactoryRef<SelectionMenuInput, MenuItemEventArg> SelectionMenu = new UIFactoryRef<SelectionMenuInput, MenuItemEventArg>();

    public static ITemporaryUI<string, object> GetPositiveFeedbackInstance() {
        return GameObjectUtil.GetResourceInstance<MessageBoxUI>("UI/PositiveFeedback");
    }

    public static ITemporaryUI<string, object> GetNegativeFeedbackInstance() {
        return GameObjectUtil.GetResourceInstance<MessageBoxUI>("UI/NegativeFeedback");
    }

}
