using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class Greeter : StaticSerializedJobGameData {

        protected override void PrepareGameData() {
            Initialize(Name);
            AddTask<StandardDialogueTask, GreeterDialogue01>("StreetSession", "Customer");
            job.AddRequirement(GetPhrase("hello"));
        }

    }
}
