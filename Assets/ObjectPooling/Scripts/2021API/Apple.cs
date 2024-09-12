using UnityEngine;

/// <summary>
/// Class to manage the clicking on apples and its properties.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Apple : MonoBehaviour {
  private float _speed = 0f;
  private GameManager _manager;
  private SpriteRenderer _renderer;
  public delegate void OnDisableCallback(Apple Instance);
  public OnDisableCallback Disable;

  void Start() {
    _manager = GameObject.Find("Managers").GetComponent<GameManager>();
    _renderer = GetComponent<SpriteRenderer>();
  }

  void Awake() {
    SetSpeed();
    _renderer = GetComponent<SpriteRenderer>();
    _renderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
  }

  void Update() {
    Move();
  }

  private void Move() {
    transform.position =
        new Vector3(transform.position.x,
            transform.position.y - _speed,
            transform.position.z);
    if (transform.position.y < -10f) {
      Deactivate();
    }
  }

  /// <summary>
  /// Handle clicks on object and add set ingame highscore.
  /// </summary>
  void OnMouseDown() {
    Deactivate();
    _manager.SetHighscore();
  }

  private void SetSpeed() {
    _speed = Random.Range(0.004f, 0.015f);
  }

  private void Deactivate() {
    Disable?.Invoke(this);
  }
}
