using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerData {

    public static bool IsInitialized {
        get {
            return PlayerManager.main;
        }
    }

    public static PlayerData Instance {
        get {
            return PlayerManager.main.playerData;
        }
    }

    public PersonalPlayerData PersonalData { get; set; }
	public int Money { get; set; }
    public bool AllowEnglish { get; set; }
	public ReviewLog ReviewLog { get; set; }
	public WordStore WordStorage { get; set; }
    public PhraseStore PhraseStorage { get; set; }
	public WordInventory WordInventory { get; set; }
	public SlotInventoryState InventoryState { get; set; }
	public LevelData LevelData { get; set; }
	public FriendsData FriendData { get; set; }
	public QuestData QuestData { get; set; }
	public string Item { get; set; }
    public ItemInventoryPlayerData ItemInventory { get; set; }
    public ConversationPlayerData Conversation { get; set; }
    public TutorialPlayerData Tutorial { get; set; }
    public LocationPlayerData Location { get; set; }
    public FlagPlayerData Flags { get; set; }

	public PlayerData(){
        AllowEnglish = false;
        PersonalData = new PersonalPlayerData();
		WordStorage = new WordStore ();
        PhraseStorage = new PhraseStore();
		WordInventory = new WordInventory ();
		LevelData = new LevelData ();
		FriendData = new FriendsData ();
		InventoryState = new SlotInventoryState ();
		QuestData = new QuestData ();
        ItemInventory = new ItemInventoryPlayerData();
        Conversation = new ConversationPlayerData();
        Tutorial = new TutorialPlayerData();
        Location = new LocationPlayerData();
        Flags = new FlagPlayerData();
		Item = "";
	}

}