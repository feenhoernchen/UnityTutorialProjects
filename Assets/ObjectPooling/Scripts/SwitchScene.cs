using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to easily switch and reload scenes.
/// </summary>
public class SwitchScene : MonoBehaviour {
    /// <summary>
    /// Simple method to switch to another scene. 
    /// </summary>
    /// <param name="sceneName">Name of scene (add all scenes to build settings first)</param>
    public void Switch(string sceneName) {
        SceneManager.LoadScene(sceneName: sceneName);
    }

    /// <summary>
    /// Simple method to reload active scene. 
    /// </summary>
    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
