using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public float speed = 2f;
    public bool isHorizontal;   //enable/disable in inspector
    public bool hitTrigger;
    public bool isMovingUp;
    public bool moving;
    private Player player;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If this moves vertically
        if (!isHorizontal)
        {
            //moving up
            if (isMovingUp && !hitTrigger)
            {
                rb.velocity = Vector2.up * speed;
            }

            //moving down
            if (!isMovingUp && !hitTrigger)
            {
                rb.velocity = Vector2.down * speed;
            }
        }

        //If this moves horizontally
        if (isHorizontal)
        {
            //moving right
            if (isMovingUp && !hitTrigger)
            {
                if(moving && !player.isMoving)
                {
                    player.rb.velocity = Vector2.right * speed;
                }
                rb.velocity = Vector2.right * speed;
            }

            //moving left
            if (!isMovingUp && !hitTrigger)
            {
                if (moving && !player.isMoving)
                {
                    player.rb.velocity = Vector2.right * -speed;
                }
                rb.velocity = Vector2.left * speed;
            }
        }
    }

    void Turn()
    {
        isMovingUp = !isMovingUp;
        hitTrigger = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "ElevatorTrigger")
        {
            hitTrigger = true; //when it hits the trigger area
            rb.velocity = Vector2.zero;

            Invoke("Turn", 0.5f);      //USEFUL, Call TURN function after X seconds (can only call functions with no parameters)
        }
    }

    
    void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.gameObject.tag == "Player")
        {
            other.collider.transform.SetParent(transform);
            moving = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.collider.transform.SetParent(null);
            moving = false;
        }
    }
}