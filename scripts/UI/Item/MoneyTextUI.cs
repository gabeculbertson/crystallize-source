using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyTextUI : MonoBehaviour {

    public Text moneyAddedText;
    public Color wrongColor;

    int lastMoney = 0;
    int shownInventoryMoney = 0;
    int remainingMoneyToAdd = 0;

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.PlayerState.OnMoneyChanged += HandleMoneyChanged;

        //moneyAddedText.gameObject.SetActive(false);
        GetComponent<Text>().text = shownInventoryMoney.ToString();
        UpdateMoney();
	}

    //void Update() {
        

    //    if (Input.GetKeyDown(KeyCode.M)) {
    //        PlayerManager.main.playerData.Money += 10;
    //        CrystallizeEventManager.main.RaiseMoneyChanged(this, System.EventArgs.Empty);
    //    }

    //    if (Input.GetKeyDown(KeyCode.N)) {
    //        PlayerManager.main.playerData.Money -= 10;
    //        CrystallizeEventManager.main.RaiseMoneyChanged(this, System.EventArgs.Empty);
    //    }
    //}

    void HandleMoneyChanged(object sender, System.EventArgs args) {
        UpdateMoney();
        //enabled = true;
        //moneyAddedText.gameObject.SetActive(true);
        //transform.localScale = 2f * Vector3.one;
    }

    void UpdateMoney() {
        remainingMoneyToAdd = PlayerManager.main.playerData.Money - lastMoney;
        lastMoney = PlayerManager.main.playerData.Money;

        StartCoroutine(AddMoneySequence());
    }

    IEnumerator AddMoneySequence() {
        if (remainingMoneyToAdd > 0) {
            moneyAddedText.color = Color.white;
            moneyAddedText.text = "+" + remainingMoneyToAdd;
        } else {
            moneyAddedText.color = wrongColor;
            moneyAddedText.text = remainingMoneyToAdd.ToString();
        }
        
        var cg = moneyAddedText.GetComponent<CanvasGroup>();
        while (cg.alpha < 1f) {
            cg.alpha += 5f * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (remainingMoneyToAdd != 0) {
            if (remainingMoneyToAdd > 0) {
                remainingMoneyToAdd--;
                shownInventoryMoney++;
                moneyAddedText.color = Color.white;
                moneyAddedText.text = "+" + remainingMoneyToAdd;
            } else if (remainingMoneyToAdd < 0) {
                remainingMoneyToAdd++;
                shownInventoryMoney--;
                moneyAddedText.color = wrongColor;
                moneyAddedText.text = remainingMoneyToAdd.ToString();
            } 

            GetComponent<Text>().text = shownInventoryMoney.ToString();
            yield return new WaitForSeconds(0.05f);
        }

        while (cg.alpha > 0) {
            cg.alpha -= 2f * Time.deltaTime;
            yield return null;
        }
    }

}
