using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public class Student : StaticSerializedJobGameData {

        protected override void PrepareGameData() {
            Initialize("Student");
            AddTask<StandardDialogueTask, StudentDialogue01>("SchoolClassroomSession", "Observer");
            AddTask<StandardDialogueTask, StudentDialogue02>("SchoolClassroomSession", "Observer");
            AddTask<StandardDialogueTask, StudentDialogue03>("SchoolClassroomSession", "Observer");

            job.TaskSelector = new OrderedSelectorGameData(new int[] { 0, 1, 2 });

            job.AddRequirement(GetPhrase("hello"));
            job.AddRequirement(GetPhrase("goodbye"));
            job.AddRequirement(GetPhrase("to be"));
        }

    }
}
