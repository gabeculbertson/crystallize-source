using UnityEngine;
using System.Collections;

public class SocialData : ScriptableObject {

	public Sprite portrait;
	public string surname;
	public string givenName;
	public string age;
	public string homeTown;
	public string occupation;
	public string status;

	public string ID { 
		get{
			return surname + givenName;
		}
	}

}
