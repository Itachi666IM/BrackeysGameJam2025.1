using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public AudioClip jumpClip;
    public AudioClip attackClip;
    float nextAttackTime;

    [Header("Player Dash")]
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] float dashAmount;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCoolDownTime;
    [SerializeField] TrailRenderer dashTrail;

    Rigidbody2D playerRb;
    BoxCollider2D playerFeetCollider;
    AudioSource audioSource;
    Animator playerAnim;
    Vector2 moveInput;
    PlayerHealth playerHealth;

    public bool canMove = true;
    public bool isAlive = true;
    bool canDoubleJump = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            if(!isAlive)
            {
                return;
            }
            if (isDashing)
            {
                return;
            }
            Run();
            FlipSprite();
            Respawn();
            TakeDamage();
            if(playerHealth.health<=0)
            {
                playerAnim.SetTrigger("Dying");
            }
        }
    }

    void OnMove(InputValue input)
    {
        if (!isAlive)
        {
            return;
        }
        if (isDashing)
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
        if (isDashing)
        {
            return;
        }
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (!canDoubleJump)
            { 
                return;
            }
            canDoubleJump = false;
        }
        else
        {
            canDoubleJump = true;
        }
        if (value.isPressed)
        {
            playerRb.velocity += new Vector2(0f, jumpSpeed);
            audioSource.PlayOneShot(jumpClip);
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
        if (isDashing)
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
        audioSource.PlayOneShot(attackClip);
        StartCoroutine(SlashEffectManager());

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.tag == "Enemy")
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        
    }

    public void NotAlive()
    {
        isAlive = false;
    }

    void TakeDamage()
    {
        if (playerRb.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            playerHealth.TakeDamage(5);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        playerHealth.health = 5;
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

    void OnDash(InputValue value)
    {
        if (!isDashing)
        {
            StartCoroutine(PlayerDash());
        }
    }

    IEnumerator PlayerDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0f;
        playerRb.velocity = new Vector2(dashAmount * transform.localScale.x, 0f);
        dashTrail.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        dashTrail.emitting = false;
        playerRb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDownTime);
        canDash = true;

    }
}
