using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] bool isHeaven;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    [Header("Player Attack")]
    [SerializeField] float attackRange;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float attackDamage;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] ParticleSystem slashEffect;
    [SerializeField] float slashDuration;
    float nextAttackTime;

    Rigidbody2D playerRb;
    BoxCollider2D playerFeetCollider;
    AudioSource jumpSource;
    Animator playerAnim;
    Vector2 moveInput;

    public bool canMove = true;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        jumpSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            if (!isAlive)
            {
                return;
            }
            Run();
            FlipSprite();
            Respawn();
            Die();
        }
    }

    void OnMove(InputValue input)
    {
        if (!isAlive)
        {
            return;
        }
        moveInput = input.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);
        playerRb.velocity = playerVelocity;

        bool playerHasHorizontalVelocity = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        playerAnim.SetBool("isRunning", playerHasHorizontalVelocity);

    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            playerRb.velocity += new Vector2(0f, jumpSpeed);
            jumpSource.Play();
            //playerAnim.SetTrigger("JumpTrigger");
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalVelocity = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalVelocity)
        {

            transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x), 1f);
        }

    }

    void Respawn()
    {
        if (!isHeaven) 
        { 
            return; 
        }
        else
        {
            if(playerRb.transform.position.y < minY)
            {
                playerRb.transform.position = new Vector2(playerRb.transform.position.x, maxY);
            }
        }

    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        if (value.isPressed)
        {
            if (Time.time > nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + timeBetweenAttacks;
            }
        }

    }

    void Attack()
    {
        if (isHeaven)
        {
            return;
        }
        playerAnim.SetTrigger("Attack");
        StartCoroutine(SlashEffectManager());

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        
    }

    void Die()
    {
        if (playerRb.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            playerAnim.SetTrigger("Dying");
            isAlive = false;
        }
    }
   

    IEnumerator SlashEffectManager()
    {
        slashEffect.Play();
        yield return new WaitForSeconds(slashDuration);
        slashEffect.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Handles.DrawWireDisc(attackPoint.position, Vector3.forward, attackRange);
    }
}
