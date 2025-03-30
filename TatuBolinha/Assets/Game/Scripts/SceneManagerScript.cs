using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    private SceneManager sceneManager;

    private void Awake() {
       // sceneManager = GetComponent<SceneManager>();
    }
    public void StartScene(string SceneName) {
        SceneManager.LoadScene(SceneName);
    }
    public void ExitGame() {
        Application.Quit();
    }
}
