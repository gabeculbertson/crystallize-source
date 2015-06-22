using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CategoryLegendUI : MonoBehaviour {

    public GameObject categoryPrefab;
    public Transform panel;

    // Use this for initialization
    void Start() {
        //var pos = transform.position + new Vector3 (4f, 4f);
        foreach (var cat in GUIPallet.Instance.GetColoredCategories()) {
            var instance = Instantiate(categoryPrefab) as GameObject;
            instance.GetComponentInChildren<Text>().text = cat.ToString();
            instance.GetComponentInChildren<Text>().font = GUIPallet.Instance.defaultFont;
            instance.GetComponentInChildren<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
            instance.transform.SetParent(panel);
            instance.GetComponent<Image>().color = GUIPallet.Instance.GetColorForWordCategory(cat);
            //instance.transform.position = pos;

            //pos.x += 168f;
        }
    }

}
