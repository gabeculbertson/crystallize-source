using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public abstract class StaticSerializedTaskGameData : StaticGameData {
        public abstract JobTaskGameData GetBaseTask();
    }

    public abstract class StaticSerializedTaskGameData<T> : StaticSerializedTaskGameData where T : JobTaskGameData, new() {
        protected T task = new T();

        public T GetTask() {
            PrepareGameData();
            return task;
        }

        public override JobTaskGameData GetBaseTask() {
            return GetTask();
        } 

        protected void Initialize(string name, string areaName, string actor) {
            task.Name = name;
            task.AreaName = areaName;
            task.Actor = new SceneObjectGameData(actor);
        }

        protected void SetProcess<V>() where V : IProcess<JobTaskRef, object> {
            task.ProcessType = new ProcessTypeRef(typeof(V));
        }

        protected void SetDialogue<V>() where V : StaticSerializedDialogueGameData, new() {
            task.Dialogue = new V().GetDialogue();
        }
    }
}