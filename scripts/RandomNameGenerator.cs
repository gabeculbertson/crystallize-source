using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomNameGenerator : MonoBehaviour {

	static string[] maleNames;

	public static string[] MaleNames {
		get {
			if(maleNames == null){
				var text = Resources.Load<TextAsset>("MaleNames");
				maleNames = text.text.Split('\n');
			}
			return maleNames;
		}
	}

	public static string GetRandomName(){
		string randomName = "";
		while (randomName == "") {
			randomName = MaleNames[Random.Range(0, MaleNames.Length)];
		}
		return randomName;
	}

}
