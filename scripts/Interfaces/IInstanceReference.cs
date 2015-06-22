using UnityEngine; 
using System.Collections; 

public interface IInstanceReference<T> where T : class {

    T Instance { get; }

}
