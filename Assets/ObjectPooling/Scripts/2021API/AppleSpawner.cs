using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class AppleSpawner : MonoBehaviour {
  [SerializeField]
  private Apple Prefab;
  private readonly System.Random _random = new(); //.Net Random
  private ObjectPool<Apple> _pool;
  private readonly float _spawnTimeInterval = 0.2f;

  private void Awake() {
    _pool = new ObjectPool<Apple>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 10, 20);
    _ = StartCoroutine(nameof(Spawn));
  }

  private Apple CreatePooledObject() {
    Apple instance = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
    instance.Disable += ReturnObjectToPool;
    instance.gameObject.SetActive(false);

    return instance;
  }

  private void ReturnObjectToPool(Apple Instance) {
    _pool.Release(Instance);
  }

  private void OnTakeFromPool(Apple Instance) {
    Instance.gameObject.SetActive(true);
    SpawnApple(Instance);
    Instance.transform.SetParent(transform, true);
  }

  private void OnReturnToPool(Apple Instance) {
    Instance.gameObject.SetActive(false);
  }

  private void OnDestroyObject(Apple Instance) {
    Destroy(Instance.gameObject);
  }

  private void OnGUI() {
    GUI.Label(new Rect(10, 40, 200, 30), $"Total Pool Size: {_pool.CountAll}");
    GUI.Label(new Rect(10, 70, 200, 30), $"Active Objects: {_pool.CountActive}");
  }

  IEnumerator Spawn() {
    for (; ; ) {
      _pool.Get();
      yield return new WaitForSeconds(_spawnTimeInterval);
    }
  }

  private void SpawnApple(Apple Instance) {
    Instance.transform.position = new Vector3(_random.Next(-14, 14), 10, 0);
  }

}
