using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float nextLevelLoadDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if(currentIndex + 1 == SceneManager.sceneCountInBuildSettings)
        {
            currentIndex = 0;
        }
        SceneManager.LoadScene(currentIndex + 1);
        yield return new WaitForSeconds(nextLevelLoadDelay);
    }
}
