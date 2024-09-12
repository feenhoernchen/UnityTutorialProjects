using UnityEngine;
using Type = System.Type;
using Object = UnityEngine.Object;

/// <summary>
/// Credits to my dear friend: Rudolf Chrispens
/// </summary>
public class SimpleManager : MonoBehaviour {
  public bool AwakeInitDone = false;
  public bool DebugMode = false;

  public static SimpleManager GET = null;
  public CustomDict<Type, Object> Singletons = new();

  /// <summary>
  /// Gets called at the beginning of the game.
  /// It adds all the managers into the singleton managers dictionary
  /// </summary>
  public virtual void SetUp() {
    if (DebugMode)
      Debug.Log("Init all Managers...");

    AddS(typeof(ObjectPool));
    AddS(typeof(LevelGenerator));

    if (DebugMode)
      Debug.Log("... finished all Managers");
  }

  public void Initialization() {
    for (int i = 0; i < Singletons.Count; i++) {
      ISingleton tSingletoninterfaceHandle = (ISingleton)Singletons.Values[i];

      if (tSingletoninterfaceHandle != null)
        tSingletoninterfaceHandle.Initialize();
      else {
        Debug.LogError("Error: " + Singletons.Values[i].ToString() +
            " does not include the " + typeof(ISingleton) + " interface! ");
      }
    }
  }

  public void ClearDictionary() {
    Singletons.Clear();
  }

  /// <summary>
  /// This is the initialization of each game start.
  /// </summary>
  void Awake() {
    if (GET == null) {
      GET = this;
    } else if (GET != this) {
      Destroy(this.gameObject);
    }

    //if there are components missing add them to the dictionary!
    SetUp();

    //create/initialize them all and add singletons to list 
    Initialization();

    AwakeInitDone = true;
  }

  /// <summary>
  /// Returns an object and casts the object into the preferred type.
  /// </summary>
  /// <param name="singletonClass">Singleton Class Type</param>
  /// <returns>The Singleton Instance</returns>
  public Object GetSingleton(Type singletonClass) {
    if (Singletons.ContainsKey(singletonClass)) {
      Singletons.TryGetValue(singletonClass, out Object OUT);
      return OUT;
    } else {
      if (DebugMode)
        Debug.LogError("Could not find referenced key in " +
            Singletons + " on static class " + this.ToString());
      return null;
    }
  }

  /// <summary>
  /// Get a casted object if system finds a generic type.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public T Singleton<T>() where T : UnityEngine.Object {
    if (Singletons.ContainsKey(typeof(T))) {
      Singletons.TryGetValue(typeof(T), out Object OUT);
      return OUT as T;
    } else {
      if (DebugMode) {
        Debug.LogError("Could not find referenced key in " +
            Singletons + " on static class " + this.ToString());
      }
      return default;
    }
  }

  /// <summary>
  /// Adds an object into a singleton dictionary. 
  /// </summary>
  /// <param name="_KEY">System.Type</param>
  /// <returns>Returns true if it was a success and false if not</returns>
  public bool AddS(Type _KEY) {
    var tObj = GetComponent(_KEY);

    if (tObj == null) {
      tObj = gameObject.AddComponent(_KEY);
    }

    if (Singletons.ContainsKey(_KEY)) {
      Singletons.ChangeValue(_KEY, tObj);
    } else {
      Singletons.Add(_KEY, tObj);
    }

    if (Singletons.ContainsKey(_KEY)) {
      return true;
    } else {
      if (DebugMode) {
        Debug.LogError("Could not add key to dictionary!");
      }
      return false;
    }
  }
}

