using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class Tourist : StaticSerializedJobGameData {

        protected override void PrepareGameData() {
            Initialize("Tourist");
            AddTask<StandardDialogueTask, GreetingDialogue01>("StreetSession", "Observer");
        }

    }
}
