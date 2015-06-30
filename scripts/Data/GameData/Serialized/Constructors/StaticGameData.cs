using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public abstract class StaticGameData {
        protected string Name {
            get {
                return GetType().Name;
            }
        }

        protected abstract void PrepareGameData();
    }
}