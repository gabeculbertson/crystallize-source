using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public class StreetDialogue01 : StaticSerializedDialogueGameData {

        protected override void PrepareGameData() {
            AddActor("Waiter");
            AddActor("Customer");

            AddLine("Hello.", 0);
            AddLine("Hello.", 1);
            AddLine("How many?", 0);
        }

    }
}