using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        batMove();
    }

    void batMove()
    {
        if (player.batTime)
        {
            rb.velocity = new Vector2(8f, 0f);
        }     
    }
}
