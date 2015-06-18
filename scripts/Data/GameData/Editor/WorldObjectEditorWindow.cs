using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class WorldObjectEditorWindow : EditorWindow {

	public static void Open(int worldID){
		var window = GetWindow<WorldObjectEditorWindow>();

		window.Initialize (worldID);
	}

	int worldID = -1;
	BranchedDialogue branchDialogue;
	BranchedDialogueHolder holder;
    LinearDialogueSection linearDialogue;
    LinearDialogueHolder linearDialogueHolder;
    NPCDialogueHolder npcDialogue;
	QuestInfoGameData quest;
	ContextData contextData;
	PhraseHolder phraseHolder;

	void Initialize(int worldID){
		this.worldID = worldID;
        linearDialogue = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(worldID);
        linearDialogueHolder = GameData.Instance.DialogueData.LinearDialogueHolders.GetItem(worldID);
        npcDialogue = GameData.Instance.DialogueData.NPCDialogueHolders.GetItem(worldID);//.GetNPCDialogueForWorldObject(worldID);
		branchDialogue = GameData.Instance.DialogueData.GetBranchedDialogueForWorldObject (worldID);
		holder = GameData.Instance.DialogueData.BranchedDialogueHolders.GetItem(worldID);
		quest = GameData.Instance.QuestData.GetQuestInfoFromWorldID (worldID);
		contextData = GameData.Instance.DialogueData.PersonContextData.GetItem (worldID);
		phraseHolder = GameData.Instance.DialogueData.PersonPhrases.GetItem (worldID);
	}

	void OnGUI(){
		EditorGUILayout.BeginVertical ();
		
		EditorGUILayout.LabelField (worldID.ToString());

        DrawButton("linear dialogue",
                   linearDialogue != null,
                   () => GameData.Instance.DialogueData.AttachNewLinearDialogue(worldID),
                   () => LinearDialogueEditorWindow.Open(linearDialogueHolder));

        DrawButton("npc dialogue",
                   npcDialogue != null,
                   () => GameData.Instance.DialogueData.AttachNewNPCDialogue(worldID),
                   () => NPCDialogueEditorWindow.Open(npcDialogue));

        DrawButton("branch dialogue",
                    branchDialogue != null,
                    () => GameData.Instance.DialogueData.AttachNewBranchedDialogue(worldID),
                    () => BranchedDialogueEditorWindow.Open(holder));

        DrawButton("quest",
                    quest != null,
                    () => GameData.Instance.QuestData.AddQuest(new QuestInfoGameData(-1, worldID)),
                    () => QuestInfoEditorWindow.Open(quest));

        DrawButton("context data",
                    contextData != null,
                    () => GameData.Instance.DialogueData.AttachNewPersonContext(worldID),
                    () => ContextDataEditorWindow.Open(contextData));

        DrawButton("phrase",
                    phraseHolder != null,
                    () => GameData.Instance.DialogueData.PersonPhrases.AddItem(new PhraseHolder(worldID)),
                    () => PhraseEditorWindow.Open(phraseHolder.Phrase));

		EditorGUILayout.EndVertical ();
	}

    bool DrawButton(string text, bool added, Action add, Action open) {
        var s = string.Format("Edit {0}...", text);
        var c = GUI.color;
        if (!added) {
            s = string.Format("Add {0}...", text);
            GUI.color = Color.gray;
        }

        var b = GUILayout.Button(s);
        if (b) {
            if (added) {
                open();
            } else {
                add();
                Initialize(worldID);
                //open();
            }
        }

        GUI.color = c;

        return b;
    }
}
