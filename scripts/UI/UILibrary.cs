﻿using UnityEngine;
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
	public static readonly UIFactoryRef<List<TextMenuItem>, TextMenuItemEventArg> TextMenu = new UIFactoryRef<List<TextMenuItem>, TextMenuItemEventArg>();
	public static readonly UIFactoryRef<List<ValuedItem>, ValuedItemEventArg> ValuedMenu = new UIFactoryRef<List<ValuedItem>, ValuedItemEventArg>();
	//UI for Cashier
	public static readonly UIFactoryRef<SelectionMenuInput, MenuItemEventArg> SelectionMenu = new UIFactoryRef<SelectionMenuInput, MenuItemEventArg>();

    public static ITemporaryUI<string, object> GetPositiveFeedbackInstance() {
        return GameObjectUtil.GetResourceInstance<MessageBoxUI>("UI/PositiveFeedback");
    }

    public static ITemporaryUI<string, object> GetNegativeFeedbackInstance() {
        return GameObjectUtil.GetResourceInstance<MessageBoxUI>("UI/NegativeFeedback");
    }

}