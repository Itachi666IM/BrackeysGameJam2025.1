using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss1 : MonoBehaviour
{
    [Header("Attack")]
    public GameObject fireBall;
    public GameObject iceSpell;
    public Transform shotPoint;
    public float attackRate;
    float nextTimeToAttack;
    public bool canAttack;

    [Header("Boss Health")]
    public int health;
    public Slider healthBar;
    public int minHealth;
    public int maxHealth;
    public ParticleSystem hitEffect;

    Transform player;
    PlayerHealth playerHealth;
    Animator anim;

    public bool isFlipped = false;
    public bool isInvulnerable = false;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioSource audioSource;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        healthBar.maxValue = maxHealth;
        healthBar.minValue = minHealth;
        anim = GetComponent<Animator>();
        playerHealth = FindObjectOfType<PlayerHealth>();
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
        if(isInvulnerable)
        {
            return;
        }
        health -= damageAmount;
        hitEffect.Play();
        audioSource.PlayOneShot(hitSound);
        if(health==500)
        {
            anim.SetBool("isEnraged", true);
            isInvulnerable = true;
        }
        if(health<=0)
        {
            Die();
        }
    }

    public void NoLongerInvulnerable()
    {
        isInvulnerable = false;
    }

    public void Die()
    {
        anim.SetTrigger("Dead");
        audioSource.PlayOneShot(deathSound);
        Debug.Log("Boss1 Dead");
        Invoke(nameof(LoadNextScene), 1f);
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

        if(Time.time > nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + attackRate;
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth.TakeDamage(1);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
