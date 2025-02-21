using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScene : MonoBehaviour
{
    PlayerHealth playerHealth;
    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if(playerHealth != null)
        {
            playerHealth.gameObject.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Ended");
    }
}
