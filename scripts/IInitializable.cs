using UnityEngine;
using System.Collections;

public interface IInitializable<T> {

    void Initialize(T param1);

}
