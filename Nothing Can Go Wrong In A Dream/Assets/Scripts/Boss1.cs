using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    public GameObject fireBall;
    public GameObject iceSpell;
    public Transform shotPoint;

    [Header("Boss Health")]
    public int health;
    public Slider healthBar;
    public int minHealth;
    public int maxHealth;

    Transform player;

    public bool isFlipped = false;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        healthBar.maxValue = maxHealth;
        healthBar.minValue = minHealth;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Boss1 Dead");
    }

    public void ShootFireBall()
    {
        Instantiate(fireBall, shotPoint.position, shotPoint.rotation);
    }

    public void ShootIceSpell()
    {
        Instantiate(iceSpell, shotPoint.position, shotPoint.rotation);
    }

    private void Update()
    {
        healthBar.value = health;
        if(health<0)
        {
            healthBar.value = minHealth;
        }
    }
}
