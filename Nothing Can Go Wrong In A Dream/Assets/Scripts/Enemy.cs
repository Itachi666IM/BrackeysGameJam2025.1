using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyHealth;
    Rigidbody2D enemyRb;
    Animator enemyAnim;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }
    void Update()
    {
        enemyRb.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            return;
        }
        moveSpeed = -1 * moveSpeed;
        FlipEnemy();
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2((Mathf.Sign(enemyRb.velocity.x)), 1f);
    }

    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;
        if(enemyHealth > 0)
        {
            enemyAnim.SetTrigger("Hit");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
