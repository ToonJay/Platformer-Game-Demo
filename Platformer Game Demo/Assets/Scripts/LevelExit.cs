using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    int touched = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && touched == 0) {
            touched += 1;
            FindObjectOfType<AudioPlayer>().PlayExitClip();
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            FindObjectOfType<GameSession>().ResetGameSession();
        } else {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
