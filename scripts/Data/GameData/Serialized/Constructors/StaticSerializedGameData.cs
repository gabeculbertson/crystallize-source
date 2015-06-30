using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public abstract class StaticSerializedGameData : StaticGameData {

        protected abstract void AddGameData();

        public void ConstructGameData() {
            PrepareGameData();
            AddGameData();
        }

    }
}