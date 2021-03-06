﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public abstract class StaticGameData {
        public string Name {
            get {
                return GetType().Name;
            }
        }

		protected int index = 0;

        protected abstract void PrepareGameData();

		protected PhraseSequence GetPhrase(string phraseKey) {
			var p = PhraseSetCollectionGameData.GetOrCreateItem(Name).GetOrCreatePhrase(index);
			GameDataInitializer.AddPhrase(Name, phraseKey, index);
            //Debug.Log(Name + "; " + index);
            index++;
			return p;
		}
    }
}