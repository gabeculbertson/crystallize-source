using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationClientData : ScriptableObject {

    public SocialData socialData;
    public FixedPlayerDialog dialog;

    public string ID {
        get {
            return name;
        }
    }

    public List<PhraseSegmentData> GetAllWords() {
        var unknownPhrases = new List<PhraseSegmentData>();
        var addedWords = new HashSet<string>();
        for (int i = 1; i < dialog.dialogPhrases.Count; i += 2) {
            foreach (var word in dialog.dialogPhrases[i].ChildPhrases) {
                if (!ObjectiveManager.main.IsPresolved(word)
                   && !addedWords.Contains(word.Text)) {
                    unknownPhrases.Add(word);
                    addedWords.Add(word.Text);
                }
            }
        }
        return unknownPhrases;
    }

    public List<PhraseSegmentData> GetMissingWords() {
        var known = new List<PhraseSegmentData>();
        foreach (var wordID in PlayerData.Instance.WordStorage.FoundWordIDs) {
            //foreach (var wordID in PlayerManager.main.playerData.InventoryState.WordIDs) {
            if (wordID != null) {
                var word = ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(wordID);
                known.Add(word);
            }
        }
        return GetMissingWords(known);
    }

    public List<PhraseSegmentData> GetMissingWords(IEnumerable<PhraseSegmentData> knownPhrases) {
        var knownPhraseSet = new HashSet<PhraseSegmentData>(knownPhrases);
        var unknownPhrases = new List<PhraseSegmentData>();
        var addedWords = new HashSet<string>();
        for (int i = 0; i < dialog.dialogPhrases.Count; i++) {
            foreach (var word in dialog.dialogPhrases[i].ChildPhrases) {
                if (!knownPhraseSet.Contains(word)
                   && !ObjectiveManager.main.IsPresolved(word)
                   && !addedWords.Contains(word.Text)) {
                    unknownPhrases.Add(word);
                    addedWords.Add(word.Text);
                }
            }
        }
        return unknownPhrases;
    }

}
