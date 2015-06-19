using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FriendsData {

	public List<FriendStateData> Friends { get; set; }

	public FriendsData (){
		Friends = new List<FriendStateData> ();
	}

	public FriendStateData GetFriendData(string id){
		var friend = (from f in Friends where f.ID == id select f).FirstOrDefault ();
		if(friend == null){
			friend = new FriendStateData(id, 0);
			Friends.Add(friend);
		}
		return friend;
	}

	public void SetFriendState(string id, int level){
		var f = GetFriendData (id);
		if (f == null) {
			f = new FriendStateData(id, level);
			Friends.Add (f);
		}
		f.FriendLevel = level;
	}

}
