using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame() {
        FindObjectOfType<AudioPlayer>().PlayClickClip();
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        FindObjectOfType<AudioPlayer>().PlayClickClip();
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
