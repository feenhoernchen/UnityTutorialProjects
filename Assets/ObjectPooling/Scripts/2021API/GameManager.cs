using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  private Text _highscore;

  private int _applesClicked = 0;

  void Start() {
    _highscore = GameObject.Find("Highscore").GetComponent<Text>();
    _highscore.text = " 0";
  }


  public void SetHighscore() {
    _applesClicked++;
    _highscore.text = " " + _applesClicked;
  }
}
