﻿using UnityEngine;
using System.Collections;

public class EffectLibrary : ScriptableObject {

    static EffectLibrary _instance;

    public static EffectLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<EffectLibrary>("EffectLibrary");
            }
            return _instance;
        }
    }

    public InteractiveDialogActorEffectSet actorEffects;
    public GameObject uiSetEffect;
    public GameObject uiEnergizeEffect;
    public GameObject uiRisingText;
    public GameObject uiClientLock;
    public GameObject uiQuestionMark;
    public GameObject uiWordCount;
    public GameObject uiExclaimationPoint;
    public GameObject uiCheckMark;
    public GameObject uiLevelUpEffect;
    public GameObject uiMessage;

    public GameObject uiStatusThoughBubble;

    public GameObject uiFadeInEffect;
    public GameObject uiFadeOutEffect;
    public GameObject uiTimeTransition;

    public GameObject interactionTargetEffect;
    public GameObject clientTargetEffect;
    public GameObject conversationCompleteEffect;
    public GameObject positiveFeedbackEffect;
    public Sprite objectiveWordShape;
    public Sprite conversationWordShape;
    public Sprite inventoryWordShape;

}
