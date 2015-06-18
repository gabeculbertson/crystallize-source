using UnityEngine;
using System.Collections;

public interface IItemDropAcceptor {

    void AcceptDrop(int itemID, GameObject obj);

}
