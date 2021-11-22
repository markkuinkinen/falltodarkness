using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public Rigidbody2D rb;
    private Player player;
    private Animator anim;
    private CapsuleCollider2D bossCapsule;
    public bool isDead;
    private int walkSpeed = 2;
    private int returnSpeed = 3;
    private int dashSpeed = 10;
    public SpriteRenderer sRend;
    public HealthBar hp;

    public bool isWalking;
    public bool chargeRight;
    public bool chargeLeft;
    public bool stunned;
    public bool timeToFlip;
    public bool inCentre;
    public bool walkingBack;
    public bool playerInSight;
    public bool isRight;
    public bool isLeft;
    public bool isReturning;
    public bool returned;
    public float timer;

    public AudioSource bossAudio;
    public AudioClip hitSound;
    public AudioClip bossDeath;

    //public CapsuleCollider2D hitbox;

    public int state = 1;
    public int bossHP = 10;

    void Start()
    {
        bossAudio = GetComponent<AudioSource>();
        bossCapsule = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        hp = FindObjectOfType<HealthBar>();
        player = FindObjectOfType<Player>();
        anim = GetComponentInChildren<Animator>();
        sRend = GetComponentInChildren<SpriteRenderer>();

        isDead = false;
        isWalking = true;
        stunned = false;
        timeToFlip = false;
        playerInSight = false;
        isRight = false;
        isLeft = false;
        chargeLeft = false;
        chargeRight = false;
        walkingBack = false;
        isReturning = false;
        returned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            doStuff();
        }
        else
        {
            player.finalDoorShowing = true;
            bossCapsule.enabled = false;
            anim.SetBool("Death", true);
            playBossDeath();
        }

    }
    
    void doStuff() //Loop is 1 - 7 then 2-7
    {
        switch (state)
        {
            case 1:
                walk();
                //checkIfPlayer();
                if (playerInSight)
                {
                    sideCheck();
                    state = 2;
                }
                break;

            case 2:
                sideCheck();
                pause(2f); //at the end of pause changes state to 3
                break;

            case 3:
                charge(); //changes state to 4 when stunned
                break;

            case 4:
                stun(2f); //end of stun changes state to 5
                break;

            case 5:
                attack(2.3f); //end of attack changes state to 6
                break;

            case 6:

                returnToCentre(3f);
                break;

            case 7:
                walkTimer(1.5f);
                break;
        }
    }

    void playBossDeath()
    {
        bossAudio.PlayOneShot(bossDeath, 0.3f);
    }

    void returnToCentre(float x) //state 6
    {
      
        timer += Time.deltaTime;
        anim.SetBool("Attack", false);
        if (timer < x && isReturning)
        {
            anim.SetBool("Walking", true);
            if (sRend.flipX == true)  
            {
                rb.velocity = new Vector2(-returnSpeed, 0);
                if (inCentre)
                {
                    isReturning = false;
                }
            } 
            else 
            { //always works when returning from the left
                rb.velocity = new Vector2(returnSpeed, 0);
                if (inCentre)
                {
                    isReturning = false;
                }
            }
        } else
        {
            timer = 0;
            isWalking = true;
            state = 7;
        }

    }

    void walkTimer(float x) //state 7
    {
        timer += Time.deltaTime;
        if (timer < x)
        {
            walk();
        }
        else
        {
            state = 2;
            timer = 0;
        }
    }

    void walk() //state 1
    {
        if (rb.velocity.x > 0)
        {
            sRend.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            sRend.flipX = true;
            sRend.transform.localPosition = new Vector2(-0.18f, 0.22f); //basically setting the pivot point for the boss which is asymmetrical 
        }
        isWalking = true;
        anim.SetBool("Walking", true);
        rb.velocity = new Vector2(walkSpeed, 0);
    }

    void pause(float x) //state 2
    {        
        timer += Time.deltaTime;
        if (timer < x)
        {
            isWalking = false;
            anim.SetBool("Walking", false);
            anim.SetBool("Idle", true);
        } 
        else
        {
            state = 3;
            timer = 0;
        }

    }

    void charge() //state 3
    {
        if (player.isRight && !stunned) //chargeLeft/Right changed in player (onTriggerEnter)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Dash", true);
            rb.velocity = new Vector2(dashSpeed, 0);
        }
        else if (player.isLeft && !stunned)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Dash", true);
            rb.velocity = new Vector2(-dashSpeed, 0);
        }
        else
        {
            state = 4;
        }

    }

    void stun(float x) //state 4
    {
        timer += Time.deltaTime;
        if (timer < x)
        {
            stunned = true;
            anim.SetBool("Dash", false);
            anim.SetBool("Stunned", true);
        }
        else
        {
            stunned = false;
            state = 5;
            timer = 0;
        }
    }
     

    void attack(float x) //state 5
    {
        if (isLeft)
        {
            flipLeft();
            bossCapsule.size = new Vector2(4.3f, 4.3f); //increasing hitbox size for attacking phase 
        } else if (isRight)
        {
            flipRight();
            bossCapsule.size = new Vector2(4.3f, 4.3f);
        }

        timer += Time.deltaTime;
        if (timer < x)
        {
            anim.SetBool("Stunned", false);
            anim.SetBool("Attack", true);
        }
        else
        {
            bossCapsule.size = new Vector2(2.2f, 3f);
            timer = 0;
            isReturning = true;
            state = 6;           
        }
    }

    void sideCheck()
    {
        if (player.isLeft && sRend.flipX == false)
        {
            flip();
        }
        else if (player.isRight && sRend.flipX == true)
        {
            flip();
        } 
    }

    void unstun()
    {
        stunned = false;
        anim.SetBool("Stunned", false);
    }


    void flipLeft()
    {
        sRend.flipX = false;
        sRend.transform.localPosition = new Vector2(1.98f, 0.22f);
    }

    void flipRight()
    {
        sRend.flipX = true;
        sRend.transform.localPosition = new Vector2(-0.18f, 0.22f);
    }

    public void flip()
    {
        if (sRend.flipX == true)
        {
            sRend.flipX = false;
            sRend.transform.localPosition = new Vector2(1.98f, 0.22f);
        }
        else
        {
            sRend.flipX = true;
            sRend.transform.localPosition = new Vector2(-0.18f, 0.22f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "AttackBox" && stunned)
        {
            if (bossHP >= 1)
            {
                bossAudio.PlayOneShot(hitSound, 0.5f);
                bossHP -= 1;
            } 
            else
            {
                bossHP -= 1;

            }
        }

        if (other.gameObject.tag == "EnemyTrigger" && isWalking)
        {
            walkSpeed = -walkSpeed;
        }

        if (other.gameObject.tag == "BossStun" && state == 3)
        {
            stunned = true;
        }
        
        if (other.gameObject.tag == "Centre")
        {
            inCentre = true;
            isWalking = true;
        }

        if (other.gameObject.tag == "RightTrigger")
        {
            isRight = true;
        }

        if (other.gameObject.tag == "LeftTrigger")
        {
            isLeft = true;
        }

        if (other.gameObject.tag == "Return")
        {
            returned = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Centre")
        {
            inCentre = false;
        }

        if (other.gameObject.tag == "RightTrigger")
        {
            isRight = false;
        }

        if (other.gameObject.tag == "LeftTrigger")
        {
            isLeft = false;
        }

        if (other.gameObject.tag == "Return")
        {
            returned = false;
        }
    }
}
