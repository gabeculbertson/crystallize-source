using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FullInventoryUI : WordInventoryUI {

    static FullInventoryUI instance = null;

    protected override int EndIndex {
        get {
            return (((words.Count - 1) / RowCount) + 2) * RowCount;
        }
    }

    protected override bool BeforeStart() {
        if (instance) {
            Destroy(gameObject);
            return false;
        }
        instance = this;
        transform.position = new Vector2(Screen.width * .5f, Screen.height * 0.5f);
        return true;
    }

    protected override bool BeforeOnDestroy() {
        if (UISystem.main) {
            UISystem.main.RemoveCenterPanel(this);
        }
        return true;
    }

    protected override void BeforeInitialize() {
        UISystem.main.AddCenterPanel(this);
    }

    public void Close() {
        Destroy(gameObject);
    }

}
