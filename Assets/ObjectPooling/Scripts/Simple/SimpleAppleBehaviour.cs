using UnityEngine;

/// <summary>
/// Class to manage the clicking on apples and its properties.
/// </summary>
public class SimpleAppleBehaviour : MonoBehaviour {
  private float _speed = 0f;
 
  void Awake() {
     SetSpeed();
  }

  void Update() {
    transform.position = new Vector3(transform.position.x, transform.position.y - _speed,
      transform.position.z);
    if (transform.position.y < -10f) {
      gameObject.SetActive(false);
    }
  }

  /// <summary>
  /// Handle clicks on object and add set ingame highscore.
  /// </summary>
  void OnMouseDown() {
    gameObject.SetActive(false);
    SimpleManager.GET.Singleton<LevelGenerator>().SetHighscore();
    SetSpeed();
  }

  private void SetSpeed() {
    _speed = UnityEngine.Random.Range(0.004f, 0.015f);
  }
}
