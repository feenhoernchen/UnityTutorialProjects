using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour, ISingleton {
  private readonly System.Random _random = new(); //.Net Random
  private readonly float _spawnTimeInterval = 0.2f;
  private Text _highscore;
  private int _applesClicked = 0;
  private readonly string _prefabName = "ObjectPooling/SimpleApple";

  IEnumerator Spawn() {
    for (; ; ) {
      SimpleManager.GET.Singleton<ObjectPool>().ActivatePositionAndTintObject(_prefabName,
          new(_random.Next(-14, 14), 10, 0), UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
      yield return new WaitForSeconds(_spawnTimeInterval);
    }
  }

  public void Initialize() {
    _highscore = GameObject.Find("Highscore").GetComponent<Text>();
    _highscore.text = " 0";
    SimpleManager.GET.Singleton<ObjectPool>().AddPool(_prefabName,
    Resources.Load(_prefabName, typeof(GameObject)) as GameObject, 10, true);
    _ = StartCoroutine(nameof(Spawn));
  }

  public void SetHighscore() {
    _applesClicked++;
    _highscore.text = " " + _applesClicked.ToString();
  }

  private void OnGUI()  {
      GUI.Label(new Rect(10, 40, 200, 30), $"Total Pool Size: {SimpleManager.GET.Singleton<ObjectPool>().GetPoolSize(_prefabName)}");
      GUI.Label(new Rect(10, 70, 200, 30), $"Active Objects: {SimpleManager.GET.Singleton<ObjectPool>().GetActiveObjects(_prefabName)}");
  }
}

