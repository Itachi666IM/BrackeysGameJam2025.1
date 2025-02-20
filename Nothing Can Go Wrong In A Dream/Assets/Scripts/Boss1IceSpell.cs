using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1IceSpell : MonoBehaviour
{
    public float speed;
    Player player;
    public int damageAmount;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            playerHealth.health -= damageAmount;
            Destroy(gameObject);
        }
    }
}
