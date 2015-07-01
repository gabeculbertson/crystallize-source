using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public class GreetingDialogue01 : StaticSerializedDialogueGameData {

        protected override void PrepareGameData() {
            AddActor("Person01");
            AddActor("Person02");

            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("hello", 0);
            AddAnimation(new GestureDialogueAnimation("Bow"), 1);
            AddLine("hello", 1);
            Debug.Log("adding to greeting");
        }

    }
}