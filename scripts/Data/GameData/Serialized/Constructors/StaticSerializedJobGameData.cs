using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {

    public abstract class StaticSerializedJobGameData : StaticSerializedGameData {
        protected JobGameData job = new JobGameData();

        protected override void AddGameData() {
            if (Application.isEditor && !Application.isPlaying) {
                //Debug.Log("Is editor");
            } else {
                int i = GameData.Instance.Jobs.GetNextKey();
                job.ID = i;
                GameData.Instance.Jobs.AddItem(job);
                GameData.CanSave = false;

                //PlayerDataConnector.UnlockJob(new JobRef(job.ID));
                //Debug.Log(job.Name + " added to GameData");
            }
        }

        protected void Initialize(string name) {
            job.Name = name;
        }

        protected void AddTask<T>() where T : StaticSerializedTaskGameData, new() {
            job.Tasks.Add(new T().GetBaseTask());
        }

        protected void AddTask<T, D>(string level, string target) 
            where T : StaticSerializedTaskGameData, new() 
            where D : StaticSerializedDialogueGameData, new() {
            var t = new T().GetBaseTask();
            t.AreaName = level;
            t.Actor = new SceneObjectGameData(target);
            var d =  new D();
            t.Dialogue = d.GetDialogue();
            t.Name = d.Name;
            job.Tasks.Add(t);
        }

    }
}