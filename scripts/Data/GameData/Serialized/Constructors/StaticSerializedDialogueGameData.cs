using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrystallizeData {
    public abstract class StaticSerializedDialogueGameData : StaticGameData {

        protected class BranchRef {
            public PhraseSequence Prompt { get; set; }
            public int Index { get; set; }

            public BranchRef() {
                Index = -1;
            }

            public BranchRef(int index) {
                Index = index;
            }

            public BranchRef(PhraseSequence prompt) : this() {
                Prompt = prompt;
            }
        }

        protected DialogueSequence dialogue = new DialogueSequence();
        protected DialogueElement lastElement;

        Dictionary<BranchDialogueElement, List<BranchRef>> branchMapping = new Dictionary<BranchDialogueElement, List<BranchRef>>();

        public DialogueSequence GetDialogue() {
            PrepareGameData();
            AfterPrepareGameData();
            return dialogue;
        }

        void AfterPrepareGameData() {
            foreach (var k in branchMapping.Keys) {
                k.Branches = new List<BranchDialogueElementLink>(
                    branchMapping[k].Select((b) => new BranchDialogueElementLink(b.Index, b.Prompt))
                    );
            }
        }

        protected void AddActor(string name) {
            dialogue.Actors.Add(new SceneObjectGameData(name));
        }

        protected void Break() {
            lastElement = null;
        }

        protected int AddLine(string phraseKey, int actorID = 0, int nextID = -10) {
            var e = dialogue.GetNewDialogueElement<LineDialogueElement>();
            e.ActorIndex = actorID;
            e.Line = new DialogueActorLine();
            e.Line.Phrase = GetPhrase(phraseKey);
            ResolveNextID(e, nextID);
            return e.ID;
        }

        protected int AddAnimation(DialogueAnimation animation, int actorID = 0, int nextID = -10) {
            var e = dialogue.GetNewDialogueElement<AnimationDialogueElement>();
            e.ActorIndex = actorID;
            e.Animation = animation;
            ResolveNextID(e, nextID);
            return e.ID;
        }

        protected int AddMessage(string message, int nextID = -10) {
            var e = dialogue.GetNewDialogueElement<MessageDialogueElement>();
            e.Message = message;
            ResolveNextID(e, nextID);
            return e.ID;
        }

        protected int AddUI<T>(UIFactoryRef<List<T>, T> factory, Func<List<T>> getItems, Func<T, int> selectNext) 
        {
            var e = dialogue.AddNewDialogueElement(new UIDialogueElement<T>(factory, getItems, selectNext));
            ResolveNextID(e, -1);
            return e.ID;
        }

        protected int AddBranch(BranchRef[] branches) {
            var e = dialogue.GetNewDialogueElement<BranchDialogueElement>();
            branchMapping[e] = new List<BranchRef>(branches);
            //e.Branches = new List<BranchDialogueElementLink>(branches);
            ResolveNextID(e, -10);
            Break();
            return e.ID;
        }

        void ResolveNextID(DialogueElement element, int nextID) {
            if (lastElement != null) {
                lastElement.DefaultNextID = element.ID;
            }
            
            if (nextID == -10) {
                lastElement = element;
            } else {
                lastElement = null;
                element.DefaultNextID = nextID;
            }
        }


    }

}