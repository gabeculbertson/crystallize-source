    using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public static class UIUtil {

    public static void GenerateChildren<T>(IEnumerable<T> collection, List<GameObject> instances, Transform parent, Func<T, GameObject> getChildInstance)
    {
        foreach (var i in instances)
        {
            GameObject.Destroy(i);
        }
        instances.Clear();

        foreach (var item in collection)
        {
            var instance = getChildInstance(item);
            instance.transform.SetParent(parent, false);
            //Debug.Log(parent + "; " + instance.transform.parent);
            instances.Add(instance);
        }
    }

    public static bool MouseOverUI() {
        if (EventSystem.current == null) {
            return false;
        }

        var raycastResults = new List<RaycastResult>();
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    public static void FadeIn(this UIMonoBehaviour ui)
    {
        ui.canvasGroup.alpha = Mathf.MoveTowards(ui.canvasGroup.alpha, 1f, 5f * Time.deltaTime);
    }

    public static void FadeOut(this UIMonoBehaviour ui)
    {
        ui.canvasGroup.alpha = Mathf.MoveTowards(ui.canvasGroup.alpha, 0, Time.deltaTime);
    }

}
