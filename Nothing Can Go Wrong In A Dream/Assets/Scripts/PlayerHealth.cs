using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public Image life;
    public TMP_Text healthText;
    private void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType<PlayerHealth>().Length;
        if (instanceCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
    }

    private void Update()
    {
        healthText.text = health.ToString();
        if(health<0)
        {
            healthText.gameObject.SetActive(false);
        }
        else
        {
            healthText.gameObject.SetActive(true);
        }
    }
}
