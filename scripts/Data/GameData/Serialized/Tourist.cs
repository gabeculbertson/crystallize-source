using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class Tourist : StaticSerializedJobGameData {

        protected override void PrepareGameData() {
            Initialize("Tourist");
            AddTask<StandardDialogueTask, GreetingDialogue01>("StreetSession", "Observer");
            AddTask<StandardDialogueTask, GreetingDialogue02>("StreetSession", "Observer");
            AddTask<StandardDialogueTask, GreetingDialogue03>("StreetSession", "Observer");
            AddTask<StandardDialogueTask, GreetingDialogue04>("StreetSession", "Observer");

            job.TaskSelector = new OrderedSelectorGameData(new int[] { 0, 1, 0, 2 });
        }

    }
}
