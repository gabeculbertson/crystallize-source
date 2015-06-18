using UnityEngine;
using System.Collections;

public class NavigationGameData {

	public SerializableDictionary<int, AreaGameData> Areas { get; set; }

	public NavigationGameData(){
		Areas = new SerializableDictionary<int, AreaGameData> ();
	}

    public void AddNewArea() {
        int i = 0;
        while (Areas.GetItem(i) != null) {
            i++;
        }

        Areas.AddItem(new AreaGameData(i));
    }

}
