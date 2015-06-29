using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class RestaurantDialogue01 : StaticSerializedDialogueGameData {

        protected override void PrepareGameData() {
            AddActor("Waiter");
            AddActor("Customer");

            AddAnimation(0, new GestureDialogueAnimation("Bow"));
            AddLine(0, "Welcome to the restaurant. How many in your party?");
            AddLine(1, "x people.");
            AddLine(0, "Please, this way.");
        }

    }
}