using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendsPanelUI : MonoBehaviour {

    public static FriendsPanelUI main { get; set; }

    const float SlideSpeed = 5000f;
    const float Padding = 4f;

    public SocialDataDictionary socialDictionary;
    public GameObject networkCardPrefab;
    public GameObject friendRequestButtonsPrefab;
    public GameObject setEffect;
    public InteractiveDialogActorEffectSet effectSet;
    public RectTransform slidingPanel;
    public bool isOpen = false;
    public List<GameObject> socialCardInstances = new List<GameObject>();

    public RectTransform rectTransform { get; private set; }
    public bool IsVisible { get; set; }

    Dictionary<string, GameObject> cardInstances = new Dictionary<string, GameObject>();

    float targetHeight = 10f;
    float currentHeight = 10f;

    void Awake() {
        main = this;
    }

    // Use this for initialization
    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        foreach (var f in PlayerData.Instance.FriendData.Friends) {
            if (!cardInstances.ContainsKey(f.ID)) {
                cardInstances[f.ID] = CreateFriendCard(socialDictionary.GetSocialData(f.ID));
                AddFriend(cardInstances[f.ID]);
            }
        }

        if (isOpen) {
            targetHeight = Screen.height - 30f;

            ArrangeInstances();
        } else {
            targetHeight = 10f;
        }
        currentHeight = Mathf.MoveTowards(currentHeight, targetHeight, SlideSpeed * Time.deltaTime);

        slidingPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentHeight);

        SetInstancesEnabled(Mathf.Approximately(currentHeight, Screen.height - 30f));
    }

    void SetInstancesEnabled(bool isEnabled) {
        foreach (var i in socialCardInstances) {
            i.SetActive(isEnabled);
        }
        IsVisible = isEnabled;
    }

    void ArrangeInstances() {
        if (socialCardInstances.Count == 0) {
            return;
        }

        var point = (Vector2)transform.position + new Vector2(rectTransform.rect.width * 0.5f, -(Padding + 30f));
        foreach (var instance in socialCardInstances) {
            var t = instance.GetComponent<RectTransform>();
            point -= t.rect.height * 0.5f * Vector2.up;
            var anchoredElement = instance.GetInterface<IAnchoredUIElement>();
            if (anchoredElement != null) {
                anchoredElement.Anchor = point;
            } else {
                instance.transform.position = point;
            }
            point -= (t.rect.height * 0.5f + Padding) * Vector2.up;
        }
    }

    public void CreateFriendRequest(SocialData socialData) {
        var instance = CreateFriendCard(socialData);

        var buttons = Instantiate(friendRequestButtonsPrefab) as GameObject;
        buttons.transform.SetParent(instance.transform);

        var effect = Instantiate(setEffect) as GameObject;
        effect.transform.SetParent(instance.transform);
    }

    GameObject CreateFriendCard(SocialData socialData) {
        var instance = Instantiate(networkCardPrefab) as GameObject;
        instance.transform.SetParent(MainCanvas.main.transform);
        instance.transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.75f);
        instance.GetComponent<NetworkCardUI>().socialData = socialData;
        return instance;
    }

    public void AddFriend(GameObject instance) {
        if (!socialCardInstances.Contains(instance)) {
            instance.transform.SetParent(transform);
            socialCardInstances.Add(instance);
        }
    }

}
