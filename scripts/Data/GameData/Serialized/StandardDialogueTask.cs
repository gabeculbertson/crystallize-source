using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class StandardDialogueTask : StaticSerializedTaskGameData<JobTaskGameData> {
        protected override void PrepareGameData() {
            Initialize("Visit the restaurant", "RestaurantTest", "Observer");
            SetProcess<StandardConversationProcess>();
        }
    }
}
