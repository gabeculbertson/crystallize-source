using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public abstract class StaticSerializedDialogueGameData : StaticGameData {

        protected class BranchRef {
            public int Index { get; set; }

            public BranchRef() {
                Index = -1;
            }

            public BranchRef(int index) {
                Index = index;
            }
        }

        protected DialogueSequence dialogue = new DialogueSequence();
        protected int index = 0;
        protected DialogueElement lastElement;

        public DialogueSequence GetDialogue() {
            PrepareGameData();
            return dialogue;
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

        protected int AddUI<T>(UIFactoryRef<List<T>, T> factory, Func<List<T>> getItems, Func<T, int> selectNext) 
        {
            var e = dialogue.AddNewDialogueElement(new UIDialogueElement<T>(factory, getItems, selectNext));
            ResolveNextID(e, -1);
            return e.ID;
        }

        protected void AddBranch(BranchDialogueElementLink[] branches) {
            var e = dialogue.GetNewDialogueElement<BranchDialogueElement>();
            e.Branches = new List<BranchDialogueElementLink>(branches);

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

        protected PhraseSequence GetPhrase(string phraseKey) {
            var p = PhraseSetCollectionGameData.GetOrCreateItem(Name).GetOrCreatePhrase(index);
            GameDataInitializer.AddPhrase(Name, phraseKey);
            index++;
            return p;
        }

    }
}