using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

class ObjectPool : MonoBehaviour, ISingleton {
  //Contains all object pools
  private readonly Dictionary<string, PoolEntity> _objectPools = new();
  //empty to group all objects
  public GameObject Empty { get; set; }

  public void Initialize() {
    Empty = new GameObject("ObjectPool");
    Empty.transform.position = new Vector3(0, 0, 0);
  }

  public int GetPoolSize(string poolName) {
    return _objectPools[poolName].PooledObjects.Count;
  }

  public int GetActiveObjects(string poolName) {
    return _objectPools[poolName].PooledObjects.Count(obj => obj.activeInHierarchy);
  }

  /// <summary>
  /// Initializes new object pool. It fills the pool with inactive objects.
  /// </summary>
  /// <param name="pool">Pool to be initialized.</param>
  private void InitializePool(PoolEntity pool) {
    for (int i = 0; i < pool.PooledAmount; i++) {
      GameObject tmp = Instantiate(pool.PooledPrefab);
      tmp.SetActive(false);
      tmp.transform.parent = pool.Empty.transform;
      pool.PooledObjects.Add(tmp);
    }
    pool.Empty.transform.parent = Empty.transform;
  }

  /// <summary>
  /// Adds a new pool to dictionary.
  /// </summary>
  /// <param name="poolName">Name of pool. Important to find pool later on.</param>
  /// <param name="prefab">Prefab to be pooled.</param>
  /// <param name="amount">Amount of pooled objects.</param>
  /// <param name="grow">Pool will grow if necessary.</param>
  public void AddPool(string poolName, GameObject prefab, int amount, bool grow) {
    PoolEntity tmp = new(prefab, amount, grow);
    InitializePool(tmp);
    _objectPools.Add(poolName, tmp);
  }

  /// <summary>
  /// Gets pool according to given name.
  /// </summary>
  /// <param name="poolName">Name of pool.</param>
  /// <returns></returns>
  public PoolEntity GetPool(string poolName) {
    if (_objectPools.TryGetValue(poolName, out PoolEntity temp)) {
      return temp;
    }

    return null;
  }

  /// <summary>
  /// Gets the next inactive element from objectpool.
  /// </summary>
  /// <param name="poolName">Name of pool.</param>
  /// <returns>GameObject or null if all Objects are active in hierarchy</returns>
  private GameObject GetInactiveObjectFromPool(string poolName) {
    PoolEntity tmp = GetPool(poolName);
    if (tmp != null) {
      for (int i = 0; i < tmp.PooledObjects.Count; i++) {
        if (!tmp.PooledObjects[i].activeInHierarchy) {
          return tmp.PooledObjects[i];
        }
      }

      if (tmp.WillGrow) {
        GameObject obj = Instantiate(tmp.PooledPrefab);
        obj.transform.parent = tmp.Empty.transform;
        tmp.PooledObjects.Add(obj);
        return obj;
      }
    }
    Debug.Log("... Couldn't get object!");
    return null;
  }

  /// <summary>
  /// Gets an object from pool and changes position, rotation and scale.
  /// </summary>
  /// <param name="poolName">string - name of pool</param>
  /// <param name="position">Vector3 - position</param>
  /// <param name="rotation">Quaternion - rotation</param>
  /// <param name="scale">Vector3 - scale</param>
  public void ActivateAndPositionObject(string poolName, Vector3 position, Quaternion? rotation = null, Vector3? scale = null) {
    GameObject tmp = GetInactiveObjectFromPool(poolName);
    if (tmp != null) {
      tmp.transform.SetPositionAndRotation(position, rotation ?? Quaternion.identity);
      tmp.transform.localScale = scale ?? new Vector3(1f, 1f, 1f);
      tmp.SetActive(true);
    } else {
      Debug.Log("No object from pool available!");
    }
  }

  /// <summary>
  /// Gets an object from pool and changes position, rotation and scale.
  /// </summary>
  /// <param name="poolName">String - name of pool</param>
  /// <param name="position">Vector3 - position</param>
  /// <param name="rotation">Quaternion - rotation</param>
  /// <param name="scale">Vector3 - scale</param>
  public void ActivateAndPositionObject(string poolName, Vector2Int position, Quaternion? rotation = null, Vector3? scale = null) {
    ActivateAndPositionObject(poolName, new Vector3(position.x, position.y, 0), rotation, scale);
  }


  /// <summary>
  /// Gets an object from pool and changes position and color.
  /// </summary>
  /// <param name="poolName">String - name of pool</param>
  /// <param name="position">Vector3 - position</param>
  /// <param name="color">Color - tint color</param>
  public void ActivatePositionAndTintObject(string poolName, Vector3 position, Color tint) {
    ActivatePositionTintAndReturnObject(poolName, position, tint);
  }

  /// <summary>
  /// Gets an object from pool and changes position and color.
  /// </summary>
  /// <param name="poolName">String - name of pool</param>
  /// <param name="position">Vector3 - position</param>
  /// <param name="color">Color - tint color</param>
  public GameObject ActivatePositionTintAndReturnObject(string poolName, Vector3 position, Color tint) {
    GameObject tmp = GetInactiveObjectFromPool(poolName);
    if (tmp != null) {
      tmp.transform.SetPositionAndRotation(position, Quaternion.identity);
      tmp.transform.localScale = new Vector3(1f, 1f, 1f);
      if (tint != null && tmp.GetComponent<SpriteRenderer>() != null) {
        tmp.GetComponent<SpriteRenderer>().color = tint;
      }
      tmp.SetActive(true);
    } else {
      Debug.Log("No object from pool available!");
    }
    return tmp;
  }

  /// <summary>
  /// Deactivates all objects from pool!
  /// </summary>
  /// <param name="poolName">String - name of pool</param>
  public void DeactivatePool(string poolName) {
    PoolEntity tmp = GetPool(poolName);
    if (tmp != null) {
      for (int i = 0; i < tmp.PooledObjects.Count; i++) {
        tmp.PooledObjects[i].SetActive(false);
      }
    } else {
      Debug.Log("Pool doesn't exist! Cannot deactivate pooled objects!");
    }
  }

  /// <summary>
  /// Deactivates all object in all pools.
  /// </summary>
  public void DeactivateAll() {
    foreach (KeyValuePair<string, PoolEntity> entry in _objectPools) {
      for (int i = 0; i < entry.Value.PooledObjects.Count; i++) {
        entry.Value.PooledObjects[i].SetActive(false);
      }
    }
  }
}
