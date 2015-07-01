using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class Wanderer : StaticSerializedJobGameData {

        protected override void PrepareGameData() {
            Initialize("Wanderer");
            AddTask<MeetOnStreet01>();
            job.Hide = true;
        }

    }
}