using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SocialDataDictionary : ScriptableObject {

	public string path = "crystallize/scriptableobjects";
	public List<SocialData> socialData = new List<SocialData>();

	public SocialData GetSocialData(string id){
		return (from sd in socialData where sd.ID == id select sd).FirstOrDefault();
	}

}
