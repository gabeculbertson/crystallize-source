using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class Eater : StaticSerializedJobGameData {

        protected override void PrepareGameData() {
            Initialize("Eater");
            AddTask<VisitRestaurant01>();
            job.AddRequirement(GetPhrase("hello"));
            //  job.AddRequirement(GetPhrase("how many people"));

            job.Hide = true;
        }

    }
}