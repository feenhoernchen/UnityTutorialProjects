using System.Collections.Generic;
using UnityEngine;

class PoolEntity {
  private List<GameObject> _pooledObjects = new();     //List of pooled GameObjects
  private GameObject _pooledPrefab;                    //the pooled prefab
  private float _pooledAmount = 0;                     //size of pool
  private bool _willGrow = true;                       //pool will extend if true
  private GameObject _empty;                           //empty to group all prefabs

  /// <summary>
  /// Constructor to fill variables
  /// </summary>
  /// <param name="prefab">prefab to be pooled</param>
  /// <param name="amount">amount of pooled objects</param>
  /// <param name="grow">will extend if true</param>
  public PoolEntity(GameObject prefab, float amount, bool grow) {
    _pooledPrefab = prefab;
    _pooledAmount = amount;
    _willGrow = grow;
    _empty = new GameObject(prefab.name);
    _empty.transform.position = new Vector3(0, 0, 0);
  }

  //getter and setter methods
  public bool WillGrow {
    get { return _willGrow; }
    set { _willGrow = value; }
  }

  public GameObject Empty {
    get { return _empty; }
    set { _empty = value; }
  }

  public float PooledAmount {
    get { return _pooledAmount; }
    set { _pooledAmount = value; }
  }

  public List<GameObject> PooledObjects {
    get { return _pooledObjects; }
    set { _pooledObjects = value; }
  }

  public GameObject PooledPrefab {
    get { return _pooledPrefab; }
    set { _pooledPrefab = value; }
  }
}
