using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IItemSelector<T> {

    event EventHandler<EventArgs<T>> OnItemSelected;

}
