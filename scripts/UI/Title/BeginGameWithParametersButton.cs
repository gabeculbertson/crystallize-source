using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BeginGameWithParametersButton : MonoBehaviour, IPointerClickHandler {

	public string level = "Tutorial_Level01";
    public InputField nameInput;
    public ColorToggleButton toggleButton;
	public bool multiplayerFlag = false;

	public void OnPointerClick (PointerEventData eventData)
	{
        var n = nameInput.text;
        if (n == "") {
            n = "No name";
        }

        foreach (var word in PlayerData.Instance.WordStorage.InventoryElements) {
            if (word == null) {
                continue;
            }

            if (word.ContainsTag("name")) {
                PlayerManager.main.playerData.WordStorage.InventoryElements.Remove(word);
                break;
            }
        }

        PlayerData.Instance.AllowEnglish = toggleButton.isOn;
		if (multiplayerFlag) {
			PlayerData.Instance.Flags.SetFlag (FlagPlayerData.IsMultiplayer, true);
		}

        var we = new PhraseSequenceElement(PhraseSequenceElementType.Text, n);
        we.AddTag("name");
        var w = PhraseSegmentData.GetWordInstance(we);
        PlayerData.Instance.PersonalData.Name = n;
        PlayerData.Instance.WordStorage.AddFoundWord(w.WordID);
        PlayerData.Instance.WordStorage.InventoryElements.Add(we);
        PlayerManager.main.Save();

        Application.LoadLevel(level);
	}

}
