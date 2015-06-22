using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixedPlayerDialog : ScriptableObject {

    public int id = -1;
    public List<PhraseSegmentData> dialogPhrases = new List<PhraseSegmentData>();
    public PhraseSegmentData successPhrase;
    public PhraseSegmentData failurePhrase;
    public PhraseSegmentData unsurePhrase;

    public List<PhraseSegmentData> GetMissingWords(List<PhraseSegmentData> missingWords) {
        if (missingWords.Count > 0) {
            return missingWords;
        }

        var known = new List<PhraseSegmentData>();
        foreach (var wordID in PlayerData.Instance.WordStorage.FoundWordIDs) {
            if (wordID != null) {
                var word = ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(wordID);
                known.Add(word);
            }
        }

        var knownPhraseSet = new HashSet<PhraseSegmentData>(known);
        var unknownPhrases = new List<PhraseSegmentData>();
        var addedWords = new HashSet<string>();
        for (int i = 0; i < dialogPhrases.Count; i++) {
            foreach (var word in dialogPhrases[i].ChildPhrases) {
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
