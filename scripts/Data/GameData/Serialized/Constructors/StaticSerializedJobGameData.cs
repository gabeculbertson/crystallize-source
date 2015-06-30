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

                PlayerDataConnector.UnlockJob(new JobRef(job.ID));
                //Debug.Log(job.Name + " added to GameData");
            }
        }

        protected void Initialize(string name) {
            job.Name = name;
        }

        protected void AddTask<T>() where T : StaticSerializedTaskGameData, new() {
            job.Tasks.Add(new T().GetBaseTask());
        }

    }
}