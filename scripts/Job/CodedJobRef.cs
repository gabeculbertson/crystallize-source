using UnityEngine;
using System.Collections;

public class CodedJobRef : JobRef {

	JobGameData gameData;
	JobPlayerData playerData;
	
	public override JobGameData GameDataInstance {
		get {
			return gameData;
		}
		set {
			gameData = value;
		}
	}
	
	public override JobPlayerData PlayerDataInstance {
		get {
			return playerData;
		}
		set {
			playerData = value;
		}
	}

	public CodedJobRef(int id) : base(id) { }

	public CodedJobRef(int id, JobGameData gameData, JobPlayerData playData) : base(id){
		GameDataInstance = gameData;
		PlayerDataInstance = playerData;
	}
}
