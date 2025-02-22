using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeavenToHell : MonoBehaviour
{
    public GameObject cutScene;
    public GameObject bgm;
    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            bgm.SetActive(false);
            cutScene.SetActive(true);
            Player player = FindObjectOfType<Player>();
            player.canMove = false;
            Invoke(nameof(LoadNextScene), 13f);
        }
    }

    void LoadNextScene()
    {
        playerHealth.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
