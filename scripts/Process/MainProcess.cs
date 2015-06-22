using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Context {
    
}

public class MainProcess {

    //class Wrapper<T> {

    //}

    //static Wrapper<T> Wrap<T>(Func<T> wrap) {
    //    return new Wrapper<T>();
    //}

    //static UIFactory<T, I, O> GetUIFactory<T, I, O>(Func<T> getter) where T : ITemporaryUI<I, O> {
    //    return new UIFactory<T, I, O>(getter);
    //}

    static void func<I,O>(Func<ITemporaryUI<I,O>> f) {

    }

    public static void Initialize() {
        LinkProcesses();
        LinkUI();

#if UNITY_EDITOR
        SetDebugState();
#endif
    }

    static void LinkProcesses() {
        // Highest level
        GameTimeProcess.MorningFactory.Set<MorningProcess>();
        GameTimeProcess.DayFactory.Set<DayProcess>();
        GameTimeProcess.EveningFactory.Set<EveningProcess>();
        GameTimeProcess.NightFactory.Set<NightProcess>();

        // Use the same transition process for all session types
        TimeSessionProcess.TransitionFactory.Set<SessionTransitionProcess>();
        //TimeSessionProcess.TransitionFactory.SetUI<SessionTransitionUI>(SessionTransitionUI.GetInstance);

        // Link main events for morning session
        MorningProcess.RequestPlanSelection.Set<JobSelectionProcess>();

        // Link main events for day session
        DayProcess.RequestJob.Set<JobProcessSelector>();

        // Link main events for evening session
        EveningProcess.RequestExplore.Set<TempProcess<object, TimeSessionArgs>>();

        // Link main events for night session
        NightProcess.RequestReviews.Set<TempProcess<object, TimeSessionArgs>>();

        // Conversation sub-processes
        ConversationSequence.RequestConversationCamera.Set<ConversationCameraProcess>();
        ConversationSequence.RequestLinearDialogueTurn.Set<LinearDialogueTurnSequence>();
        ConversationSequence.RequestPromptDialogueTurn.Set<PromptDialogueTurnSequence>();
        PromptDialogueTurnSequence.RequestPhrasePanel.Set<PhraseSelectionProcess>();
        PhraseSelectionProcess.RequestPhraseEditor.Set<EditPhraseProcess>();

        // Library of misc reusable processes
        ProcessLibrary.Conversation.Set<ConversationSequence>();
        ProcessLibrary.MessageBox.Set<MessageBoxProcess>();
    }

    static void LinkUI() {
        UILibrary.MessageBox.Set(MessageBoxUI.GetInstance);
        UILibrary.SessionTransition.Set(SessionTransitionUI.GetInstance);
        UILibrary.PhraseEditor.Set(ReplaceWordPhraseEditorUI.GetInstance);
        UILibrary.WordSelector.Set(WordSelectionPanelUI.GetInstance);
        UILibrary.ConversationCamera.Set(ConversationCameraController.GetInstance);
        UILibrary.ContextActionButton.Set(ContextActionButtonUI.GetInstance);
        UILibrary.PositiveFeedback.Set(UILibrary.GetPositiveFeedbackInstance);
        UILibrary.NegativeFeedback.Set(UILibrary.GetNegativeFeedbackInstance);
        UILibrary.MoneyState.Set(MoneyPanelUI.GetInstance);
        UILibrary.HomeSelectionPanel.Set(HomeSelectionPanelUI.GetInstance);
        UILibrary.HomeShopPanel.Set(HomeShopUI.GetInstance);
        UILibrary.SkipSessionButton.Set(SkipSessionButtonUI.GetInstance);
    }

    static void SetDebugState() {
        Debug.Log("Set debug state.");
        PlayerDataConnector.UnlockJob(new JobRef(2));
        PlayerDataConnector.UnlockJob(new JobRef(1));
    }

    public static void InstantiateNewSceneObjects() {
        CrystallizeEventManager.GetInstance();
        UISystem.GetInstance();
        SpeechPanelUI.GetInstance();
        GameEventHandler.GetInstance();
    }

}
