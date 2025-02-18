using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1FireBall : MonoBehaviour
{
    public float speed;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
