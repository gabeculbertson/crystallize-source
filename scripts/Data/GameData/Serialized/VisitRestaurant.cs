﻿using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class VisitRestaurant01 : StaticSerializedTaskGameData {

        protected override void PrepareGameData() {
            Initialize("Visit the restaurant", "RestaurantTest", "Observer");
            SetProcess<RestaurantProcess>();
            SetDialogue<RestaurantDialogue01>();
        }

    }
}