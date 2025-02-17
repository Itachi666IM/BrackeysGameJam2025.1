using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] bool isHeaven;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    Rigidbody2D playerRb;
    BoxCollider2D playerFeetCollider;
    AudioSource jumpSource;
    Animator playerAnim;
    Vector2 moveInput;


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
        Run();
        FlipSprite();
        Respawn();
    }

    void OnMove(InputValue input)
    {
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
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            playerRb.velocity += new Vector2(0f, jumpSpeed);
            jumpSource.Play();
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
}
