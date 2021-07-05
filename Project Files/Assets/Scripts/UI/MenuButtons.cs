using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame() {
        SceneManager.LoadScene("scene_1");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
