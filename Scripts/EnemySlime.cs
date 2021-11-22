using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{

    public int points; //amount of points rewarded to a player

    public float speed = -1.5f;
    public float deathTime = 1.5f;

    public bool isDead;
    public bool isMoving;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D hitCollider;
    private PolygonCollider2D slimeHead;
    private Player player;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hitCollider = GetComponentInChildren<BoxCollider2D>();
        slimeHead = GetComponent<PolygonCollider2D>();
        player = FindObjectOfType<Player>();
    }

    void FixedUpdate()
    {
        if (!isDead && isMoving)
        {
            rb.velocity = new Vector2(speed, 0f);
            anim.SetBool("Moving", true);
        } 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "EnemyTrigger" || other.gameObject.tag == "Player") && !isDead) //if enemy hits trigger or player corpse
        {
            speed = -speed; //change the moving direction
            //transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f); //change to -1 to change the x of the box collider if it isnt symmetrical
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player" && !player.isDead && !isDead) //if player and slime are alive and they collide
        {
            UIController.score += points; // rewarding the player
            rb.velocity = new Vector2(0f, 0f);
            hitCollider.isTrigger = false;
            hitCollider.enabled = false;
            slimeHead.enabled = false;
            anim.SetBool("Moving", false);
            isDead = true;

            player.slimeBounce(); //function in player script that makes player bounce off slime upon killing it

            anim.SetTrigger("Death");
            Destroy(gameObject, deathTime); //deathtime aligns with slime death animation 
            
        }
    }

}
