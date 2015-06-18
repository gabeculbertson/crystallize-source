using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public enum InteractionType {
    None,
    SinglePhrase,
    LinearDialogue,
    NPCDialogue,
    BranchDialogue,
    StartQuest
}

public class DialogueSystemManager : MonoBehaviour {

    //TODO: we don't want this
    public static DialogueSystemManager main { get; set; }

    public GameObject InteractionTarget {
        get {
            if (!interactionTarget) {
                return null;
            }
            return interactionTarget.gameObject;
        }
    }

    public InteractionType Mode {
        get {
            return interactionMode;
        }
    }

    GameObject interactionMenuParent;

    List<Component> engagedTargets = new List<Component>();

    //TODO: we probably don't want to keep this
    InteractionType interactionMode = InteractionType.None;
    Component interactionTarget;
    IEnumerator<DialogueActorLine> linearDialoguePosition;
    public BranchedDialogueElement branch;
    PhraseSegmentData phrase;

    //GameObject selectionEffectInstance;

    void Awake() {
        main = this;
    }

    // Use this for initialization
    void Start() {
        if (GameSettings.GetFlag(GameSystemFlags.LockDialogueSystem)) {
            return;
        }

        CrystallizeEventManager.Environment.OnActorApproached += HandleOnActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted += HandleOnActorDeparted;
        CrystallizeEventManager.UI.OnUIInteraction += HandleOnUIInteraction;
    }

    void HandleOnActorApproached(object sender, System.EventArgs e) {
        //interactionTarget = (Component)sender;
        engagedTargets.Add((Component)sender);
    }

    void HandleOnActorDeparted(object sender, System.EventArgs e) {
        engagedTargets.Remove((Component)sender);
    }

    void UpdateInteractionTarget() {
        if (engagedTargets.Count == 0) {
            if (interactionTarget != null) {
                CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Exploring));
                //UISystem.main.Mode = UIMode.Exploring;
                EndInteraction();
            }
        } else {
            Component nearest = null;
            var shortest = Mathf.Infinity;
            foreach (var t in engagedTargets) {
                var d = Vector3.Distance(t.transform.position, PlayerManager.main.transform.position);
                if (d < shortest) {
                    nearest = t;
                    shortest = d;
                }
            }

            if (nearest != interactionTarget) {
                EndInteraction();
                SetInteractionTarget(nearest);
            }
        }
    }

    void SetInteractionTarget(Component target) {
        interactionTarget = target;
        var gid = interactionTarget.GetWorldID();
        BeginInteraction(gid);
        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));
        //UISystem.main.Mode = UISystem.UIMode.Speaking;

        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new PersonApproachedEventArgs(gid));

        DataLogger.LogTimestampedData("BeginInteraction", gid.ToString());
    }

    void Update() {
        UpdateInteractionTarget();

        //if (!selectionEffectInstance) {
        //    selectionEffectInstance = Instantiate<GameObject>(EffectManager.main.interactionTargetEffect);
        //}

        //if (interactionTarget) {
        //    selectionEffectInstance.SetActive(true);
        //    //if (interactionTarget is DialogueActor) {
        //        var target = ActorTracker.GetActorSpeechBubbleTarget(interactionTarget.transform);
        //        selectionEffectInstance.transform.position = target.position - Vector3.up * 1f;
        //    //}
        //} else {
        //    selectionEffectInstance.SetActive(false);
        //}
    }

    void HandleOnUIInteraction(object sender, System.EventArgs e) {
        switch (interactionMode) {
            case InteractionType.LinearDialogue:
                if (e is DialogueContinueRequestedEventArgs) {
                    ContinueLinearDialogue();
                }

                if (e is PhraseInputEventArgs) {
                    PlayerData.Instance.Tutorial.SetPhraseProgressionInteraction(((PhraseInputEventArgs)e).Phrase, interactionTarget.GetWorldID());
                    ContinueLinearDialogue();
                }
                break;

            case InteractionType.NPCDialogue:
                break;

            case InteractionType.BranchDialogue:
                if (e is DialogueBranchSelectedEventArgs) {
                    branch = ((DialogueBranchSelectedEventArgs)e).DialogueBranch;
                    var phrase = branch.PromptPhrase.Phrase;
                    if (phrase.PhraseElements.Count == 0) {
                        var phraseClass = GameData.Instance.PhraseClassData.GetPhraseClassFromID(branch.PromptPhraseClassID);
                        if (phraseClass.IsEmpty) {
                            Debug.LogWarning("No template entries for " + phraseClass.Description + ". exiting");
                            return;
                        }
                        var template = phraseClass.GetTemplates()[0];
                        phrase = template.Phrase;

                        CrystallizeEventManager.UI.RaiseUIRequest(this, new PhraseInputUIRequestEventArgs(interactionMenuParent, phrase));
                    } else {
                        CrystallizeEventManager.UI.RaiseUIRequest(this, new PhraseInputUIRequestEventArgs(interactionMenuParent, branch.PromptPhrase));
                    }
                } else if (e is PhraseInputEventArgs) {
                    var pargs = (PhraseInputEventArgs)e;
                    var r = branch.Responese;
                    if (r == null) {
                        r = new ResponseBehaviorGameData();
                    }
                    r.DoResponse(pargs.Phrase, branch.ResponseLine, interactionTarget.gameObject, pargs.ContextData);
                    PlayerData.Instance.Tutorial.SetPhraseProgressionInteraction(branch.PromptPhrase.Phrase, interactionTarget.GetWorldID());

                    var bd = GameData.Instance.DialogueData.GetBranchedDialogueForWorldObject(interactionTarget.GetWorldID());
                    //Debug.Log("Index: " + bd.Elements.IndexOf(branch));
                    PlayerData.Instance.Conversation.SetBranchComplete(interactionTarget.GetWorldID(), bd.Elements.IndexOf(branch));
                    // TODO: actually put something here
                    CrystallizeEventManager.PlayerState.RaiseGameEvent(this, System.EventArgs.Empty);

                    //var t 
                    //EndInteraction();
                    BeginBranchedInteraction(interactionTarget.GetWorldID());
                }
                break;

            case InteractionType.StartQuest:
                if (e is QuestConfirmedEventArgs) {
                    var questID = ((QuestConfirmedEventArgs)e).QuestID;
                    QuestManager.main.ActiveQuestID = questID;
                    PlayerManager.main.playerData.QuestData.SetQuestState(questID, ObjectiveState.Active);
                    EffectManager.main.PlayMessage("Quest started!", Color.yellow);
                    interactionMode = InteractionType.None;

                    CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this,
                                                                    new QuestStateChangedEventArgs(PlayerManager.main.PlayerID,
                                                                                                   PlayerManager.main.playerData.QuestData.GetOrCreateQuestInstance(questID)));

                    var it = InteractionTarget;
                    EndInteraction();
                    SetInteractionTarget(it.transform);
                }
                break;
        }
    }

    void GetNewInteractionParent() {
        DestroyInteractionParent();

        interactionMenuParent = new GameObject("InteractionMenu");
        interactionMenuParent.transform.SetParent(FieldCanvas.main.transform);
    }

    void DestroyInteractionParent() {
        if (interactionMenuParent) {
            Destroy(interactionMenuParent);
            interactionMenuParent = null;
        }
    }

    InteractionType GetInteractionType(int worldID) {
        if (worldID == -1) {
            return InteractionType.None;
        }

        //if (QuestManager.HasNewQuest (worldID)) {
        //    return InteractionType.StartQuest;
        //}

        var qd = GameData.Instance.QuestData.GetQuestInfoFromWorldID(worldID);
        if (qd != null) {
            var qi = qd.GetQuestInstance();
            if (qi == null) {
                return InteractionType.StartQuest;
            } else if (qi.State == ObjectiveState.Available || qi.State == ObjectiveState.Hidden) {
                return InteractionType.StartQuest;
            }
        }

        var ld = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(worldID);
        if (ld != null) {
            return InteractionType.LinearDialogue;
        }

        var nd = GameData.Instance.DialogueData.GetNPCDialogueForWorldObject(worldID);
        if (nd != null) {
            return InteractionType.NPCDialogue;
        }

        var bd = GameData.Instance.DialogueData.GetBranchedDialogueForWorldObject(worldID);
        if (bd != null) {
            return InteractionType.BranchDialogue;
        }

        var phr = GameData.Instance.DialogueData.PersonPhrases.GetItem(worldID);
        if (phr != null) {
            return InteractionType.SinglePhrase;
        }

        if (qd != null) {
            return InteractionType.StartQuest;
        }

        return InteractionType.None;
    }

    void BeginInteraction(int worldID) {
        GetNewInteractionParent();

        interactionMode = GetInteractionType(worldID);
        //Debug.Log("Interaction: " + interactionMode);
        switch (interactionMode) {
            case InteractionType.LinearDialogue:
                BeginLinearDialogue(worldID);
                break;

            case InteractionType.NPCDialogue:
                BeginNPCDialogue(worldID);
                break;

            case InteractionType.BranchDialogue:
                var p = new PhraseSequence();
                var w = new PhraseSequenceElement(PhraseSequenceElementType.Text, "?");
                p.Add(w);
                interactionTarget.GetComponent<DialogueActor>().SetPhrase(p);

                BeginBranchedInteraction(worldID);
                break;

            case InteractionType.StartQuest:
                BeginQuestConfirmationInteraction(worldID);
                break;

            case InteractionType.SinglePhrase:
                BeginSinglePhraseInteraction(worldID);
                break;
        }
    }

    void EndInteraction() {
        PlayerManager.main.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(null);
        DestroyInteractionParent();
        interactionTarget = null;

        DataLogger.LogTimestampedData("EndInteraction");
    }

    void BeginLinearDialogue(int worldID) {
        var d = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(worldID);
        if (!PlayerManager.main.playerData.Conversation.IsAvailable(d.ID)) {
            return;
        }

        if (d.GiveObjectives) {
            foreach (var nw in d.GetNeededWords()) {
                PlayerManager.main.playerData.WordStorage.AddObjectiveWord(nw.WordID);
            }
            CrystallizeEventManager.UI.RaiseUpdateUI(this, System.EventArgs.Empty);
        }

        linearDialoguePosition = new LinearDialogueEnumerator(d);
        linearDialoguePosition.MoveNext();
        ContinueLinearDialogue();
    }

    void ContinueLinearDialogue() {
        var line = linearDialoguePosition.Current;
        bool hasNext = linearDialoguePosition.MoveNext();
        if (line is NPCActorLine) {
            var cd = GameData.Instance.DialogueData.PersonContextData.GetItem(interactionTarget.GetWorldID());
            if (hasNext) {
                var nextLine = linearDialoguePosition.Current;
                if (nextLine is NPCActorLine) {
                    interactionTarget.GetComponent<DialogueActor>().SetLine(line, cd, true);
                } else {
                    interactionTarget.GetComponent<DialogueActor>().SetLine(line, cd);
                    ContinueLinearDialogue();
                }
            } else {
                interactionTarget.GetComponent<DialogueActor>().SetLine(line, cd, false);
                CompleteLinearDialogue();
            }
        } else if (line is PlayerActorLine) {
            CrystallizeEventManager.UI.RaiseUIRequest(this, new PhraseInputUIRequestEventArgs(interactionMenuParent, (PlayerActorLine)line));
        } else if (line == null) {
            CompleteLinearDialogue();
        }
    }

    void CompleteLinearDialogue() {
        //var c = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(interactionTarget.GetWorldID());
        PlayerManager.main.playerData.Conversation.SetConversationComplete(interactionTarget.GetWorldID());//c.ID);
        EffectManager.main.PlayMessage("Conversation complete!", Color.yellow);

        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, System.EventArgs.Empty);
    }

    void BeginNPCDialogue(int worldID) {
        var nd = GameData.Instance.DialogueData.GetNPCDialogueForWorldObject(worldID);
        interactionTarget.GetComponent<DialogueGroup>().BeginDialogue(nd);
    }

    void BeginBranchedInteraction(int worldID) {
        var bd = GameData.Instance.DialogueData.GetBranchedDialogueForWorldObject(worldID);

        branch = null;
        //// TODO: game event should not be here
        //CrystallizeEventManager.main.RaiseGameEvent(this, new BranchUIRequestEventArgs(interactionMenuParent, bd.Elements));
        CrystallizeEventManager.UI.RaiseUIRequest(this, new BranchUIRequestEventArgs(interactionMenuParent, bd.Elements));
    }

    void BeginQuestConfirmationInteraction(int worldID) {
        var q = GameData.Instance.QuestData.GetQuestInfoFromWorldID(worldID);
        interactionTarget.GetComponent<DialogueActor>().SetLine(q.QuestPromptLine);

        var aq = PlayerManager.main.playerData.QuestData.GetQuestInstance(QuestManager.main.ActiveQuestID);
        if (LevelSettings.main.isMultiplayer) {
            if (aq != null) {
                if (aq.State != ObjectiveState.Complete) {
                    return;
                }
            }
        }

        if (!q.IsAvailable()) {
            return;
        }

        if (QuestManager.HasNewQuest(worldID)) {
            var quest = GameData.Instance.QuestData.GetQuestInfoFromWorldID(worldID);

            CrystallizeEventManager.UI.RaiseUIRequest(this, new QuestConfirmationUIRequestEventArgs(interactionMenuParent, quest));
        }
    }

    void BeginSinglePhraseInteraction(int worldID) {
        var phr = GameData.Instance.DialogueData.PersonPhrases.GetItem(worldID);
        var l = new NPCActorLine();
        l.Phrase = phr.Phrase;

        interactionTarget.GetComponent<DialogueActor>().SetLine(l);
    }

}
