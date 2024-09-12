using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Dictionary that is Unity serializable.
/// Simply two lists. Missing some common methods.
/// </summary>
/// <typeparam name="Key">identifier</typeparam>
/// <typeparam name="Value">the actual value</typeparam>
public class CustomDict<Key, Value> {
  [SerializeField]
  private List<Key> _keys = new();

  [SerializeField]
  private List<Value> _values = new();

  public int Count {
    get { return TheCount(); }
  }

  public List<Key> Keys {
    get { return _keys; }
  }

  public List<Value> Values {
    get { return _values; }
  }

  public bool ContainsKey(Key key) {
    return _keys.Contains(key);
  }

  public void Add(Key key, Value value) {
    if (_keys.Contains(key)) {
      Debug.LogError("Dictionary already contains key! " + key.ToString());
      return;
    }

    _keys.Add(key);
    _values.Add(value);
  }

  public void Clear() {
    _keys.Clear();
    _values.Clear();
  }

  public void Remove(Key key) {
    if (!_keys.Contains(key))
      return;

    int index = _keys.IndexOf(key);
    _keys.RemoveAt(index);
    _values.RemoveAt(index);
  }

  public bool TryGetValue(Key key, out Value value) {
    if (_keys.Count != _values.Count) {
      _keys.Clear();
      _values.Clear();
      value = default;
      return false;
    }

    if (!_keys.Contains(key)) {
      value = default;
      return false;
    }

    value = _values[_keys.IndexOf(key)];

    return true;
  }

  int TheCount() {
    if (_keys.Count != _values.Count)
      Debug.LogError("Error " + this.ToString()
        + " does not have the same key and value count! "
        + _keys.Count + " notEquals " + _values.Count);

    return _values.Count;
  }

  public void ChangeValue(Key key, Value value) {
    if (!_keys.Contains(key))
      return;

    _values[_keys.IndexOf(key)] = value;
  }
}

