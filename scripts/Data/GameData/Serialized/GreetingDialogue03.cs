using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystallizeData {
    class GreetingDialogue03 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Person01");
            AddActor("Person02");

            AddMessage("During your touring, you overhear two people talking.");
            AddLine("How do you do?", 0);
            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("I'm Sakura.", 0);
            AddLine("How do you do?", 1);
            AddAnimation(new GestureDialogueAnimation("Bow"), 1);
            AddLine("I'm Chie.", 1);
            AddLine("Nice to meet you", 0);
            AddLine("The same to you.", 1);
            AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }
}
