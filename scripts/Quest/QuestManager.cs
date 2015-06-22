using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;

public class QuestManager : MonoBehaviour {

    public static QuestManager main { get; set; }

    public static bool HasNewQuest(int worldID) {
        var questInfo = GameData.Instance.QuestData.GetQuestInfoFromWorldID(worldID);
        if (questInfo == null) {
            return false;
        }

        var questState = PlayerData.Instance.QuestData.GetOrCreateQuestInstance(questInfo.QuestID);
        if (questState.State == ObjectiveState.Complete || questState.State == ObjectiveState.Active) {
            return false;
        }

        return true;
    }

    Dictionary<int, Transform> clients = new Dictionary<int, Transform>();

    Vector3 target = Vector3.zero;

    public int ActiveQuestID {
        get {
            return PlayerData.Instance.QuestData.ActiveQuest;
        }
        set {
            if (value != PlayerData.Instance.QuestData.ActiveQuest) {
                PlayerData.Instance.QuestData.ActiveQuest = value;
                UpdateQuestData();
                CrystallizeEventManager.PlayerState.RaiseActiveQuestChanged(this, new QuestEventArgs(value));
            }
        }
    }

    void Awake() {
        main = this;
    }

    IEnumerator Start() {
        yield return null;
        yield return null;

        UpdateQuestData();

        CrystallizeEventManager.PlayerState.OnGameEvent += HandleGameEvent;
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleOnQuestStateChanged;
    }

    void HandleOnQuestStateChanged(object sender, System.EventArgs e) {
        UpdateQuestData();
        //Debug.Log ("Updating");
    }

    void HandleGameEvent(object sender, System.EventArgs e) {
        foreach (var q in PlayerData.Instance.QuestData.QuestInstances) {
            if (q.State == ObjectiveState.Active) {
                q.GetQuestGameData().ReceiveMessage(e);
                if (q.State == ObjectiveState.Complete) {
                    foreach (var r in q.GetQuestGameData().Rewards) {
                        r.GrantReward();
                    }
                }
            }
        }
    }

    void UpdateQuestData() {
        if (ActiveQuestID < 100) {
            return;
        }

        var questGD = GameData.Instance.QuestData.Quests.GetItem(ActiveQuestID);
        var questState = PlayerData.Instance.QuestData.GetOrCreateQuestInstance(questGD.QuestID);
        var objective = questState.GetCurrentObjective();
        if (objective == -1) {
            //Debug.Log ("Quest finished.");
            return;
        } else {
            //Debug.Log("Objective: " + objective);
        }
        //var objective = quest.

        //Debug.Log ("Client: " + questGD.WorldID + "; " + client);

        //switch (questGD.Objectives [objective].LocationType) {
        //case ObjectiveLocationType.Self:
        //    target = GetTargetPosition (client);
        //    break;
        //case ObjectiveLocationType.Client:
        //    var tt = WorldObjectExtensions.GetWorldObject(questGD.WorldID);
        //    if(tt != null){
        //        target = tt.position;
        //    } else {
        //        target = Vector3.zero;
        //    }
        //    //transform.GetWorldID();
        //    //var cname = questGD.Objectives[objective].ClientName;
        //    //var tc = GameData.Instance.QuestData.GetClientFromName(cname);
        //    //Debug.Log(tc + "; " + cname);
        //    //target = GetTargetPosition (tc);
        //    break;
        //default:
        //    target = Vector3.zero;
        //    break;
        //}
    }

    Vector3 GetTargetPosition(QuestClientGameData client) {
        if (client.AreaID == LevelSettings.main.areaID) {
            return client.Position;
        } else {
            var path = GetAreaPath(client.AreaID);
            if (path != null) {
                var area = GameData.Instance.NavigationData.Areas.GetItem(LevelSettings.main.areaID);
                var conn = area.GetConnection(path[path.Count - 2]);
                if (conn != null) {
                    return conn.Position;
                } else {
                    Debug.Log("Unable to find connection.");
                }
            }
        }
        return Vector3.zero;
    }

    public void RegisterClient(int questID, Transform client) {
        clients[questID] = client;
    }

    public Transform GetClient(int questID) {
        if (clients.ContainsKey(questID)) {
            return clients[questID];
        }
        return null;
    }

    public int GetActiveObjective() {
        var questState = PlayerData.Instance.QuestData.GetQuestInstance(ActiveQuestID);
        var objectiveIndex = 0;
        for (int i = 0; i < questState.ObjectiveStates.Count; i++) {
            if (!questState.GetObjectiveState(i).IsComplete) {
                break;
            }
            objectiveIndex++;
        }
        return objectiveIndex;
    }

    public bool HasActiveTarget() {
        //		var info = QuestInfo.GetQuestInfo (ActiveQuestID);
        //		var obj = GetActiveObjective ();
        //		if (obj < info.Objectives.Count) {
        //			return info.Objectives[obj].LocationType != ObjectiveLocationType.None;
        //		} else {
        //			return false;
        //		}
        return target != Vector3.zero;
    }

    public Vector3 GetActiveTarget() {
        return target;
    }

    List<int> GetAreaPath(int destinationAreaID) {
        var originNode = new PathNode<int>(LevelSettings.main.areaID);
        var searchedAreas = new HashSet<int>();
        var open = new Queue<PathNode<int>>();
        open.Enqueue(originNode);
        while (open.Count > 0) {
            var node = open.Dequeue();
            searchedAreas.Add(node.Node);

            var area = GameData.Instance.NavigationData.Areas.GetItem(node.Node);
            if (area != null) {
                foreach (var conn in area.Connections) {
                    var neighbor = GetAreaPathNode(conn, destinationAreaID);
                    node.Neighbors.Add(neighbor);

                    if (!searchedAreas.Contains(neighbor.Node)) {
                        open.Enqueue(neighbor);
                    }
                }
            }
        }

        var p = Path<int>.FindPath(originNode, (i1, i2) => 1f, (i1) => 1f);
        if (p == null) {
            return null;
        }
        return new List<int>(p);
    }

    PathNode<int> GetAreaPathNode(AreaConnectionGameData area, int destination) {
        return new PathNode<int>(area.AreaID, destination == area.AreaID);
    }

}
