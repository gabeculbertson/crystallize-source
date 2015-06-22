using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class InventoryExperienceUI : MonoBehaviour, IExperienceUI {

    //public SlotInventoryPanelUI inventoryPanel;
    public RectTransform energyTarget;
    public Text experienceText;
    public Text levelText;

    public float targetEnergy = 0.5f;
    public float currentEnergy = 0.5f;

    // Use this for initialization
    void Start() {
        if (!LevelSettings.main.allowLevelUp) {
            gameObject.SetActive(false);
            return;
        }

        TutorialCanvas.main.ExperienceUI = this;
    }

    // Update is called once per frame
    void Update() {
        var inventoryState = PlayerData.Instance.InventoryState;
        targetEnergy = (float)inventoryState.CurrentLevelExperience / inventoryState.NextLevelExperience;

        if (currentEnergy >= 1f) {
            LevelUp();
        }

        currentEnergy = Mathf.MoveTowards(currentEnergy, targetEnergy, Time.deltaTime);
        energyTarget.localScale = new Vector3(currentEnergy, 1f, 1f);
        levelText.text = PlayerData.Instance.InventoryState.Level.ToString();
        experienceText.text = string.Format("{0}/{1}",
                                            (int)(currentEnergy * PlayerData.Instance.InventoryState.NextLevelExperience),
                                            PlayerData.Instance.InventoryState.NextLevelExperience);

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            EffectManager.main.EnqueueEffect(LevelUpEffect, 3f);
        }
        //energyTarget.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 0, 0);//.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
    }

    void LevelUp() {
        var inventoryState = PlayerData.Instance.InventoryState;
        inventoryState.CurrentLevelExperience -= inventoryState.NextLevelExperience;
        inventoryState.Level++;
        inventoryState.NextLevelExperience += 10;
        currentEnergy = 0;

        EffectManager.main.EnqueueEffect(LevelUpEffect, 3f);
        EffectManager.main.CreateFlashEffect(GetComponent<RectTransform>());

        //CrystallizeEventManager.main.RaiseLevelUp (this, EventArgs.Empty);
    }

    void LevelUpEffect() {
        AudioManager.main.PlayLevelUp();
        var go = Instantiate(EffectLibrary.Instance.uiLevelUpEffect) as GameObject;
        go.transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        go.transform.SetParent(MainCanvas.main.transform);
    }

}
