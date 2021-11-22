using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const int maxJumps = 1;
    
    public int jumpsLeft;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float speed = 6f;
    public float jumpVelocity = 6f;
    public float invTimer;
    public float attackTimer = 0f;
    public float flickerTimer;
    public float alphaLevel = 0.8f;

    public bool tut1;
    public bool tut2;
    public bool tut3;
    public bool isDead;
    public bool levelFinished;
    public bool forcedContinue;
    public bool canControl;
    public bool inSight;
    public bool onVerticalLift;
    public bool onHorizontalLift;
    public bool invincible;
    public bool treasureRoom;
    public bool exitTreasureRoom;
    public bool batTime;
    public bool canPort;
    public bool canReturn;
    public bool checkPointReached;
    public bool isRight; //R/L for the boss
    public bool isLeft;
    public bool finalDoorShowing;
    public bool canAttack;
    public bool bounce;
    public bool isMoving;
    public bool skeletonMode;
    private bool isOrange;

    //GroundCheck + wall check
    public bool isGrounded = false; //testing
    [SerializeField] Transform groundCheckCollider;
    const float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    public bool isWalled = false;
    [SerializeField] Transform wallCheckCollider;
    const float wallCheckRadius = 0.9f;

    //Physical Materials
    public PhysicsMaterial2D sticky;
    public PhysicsMaterial2D slippery;

    //audio
    public AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip swingSound;
    public AudioClip pickupSound;
    public AudioClip powerupSound;
    public AudioClip switchSound;
    public AudioClip recoilSound;
    public AudioClip slimeSound;
    public AudioClip spikeSound;

    //Components
    public Rigidbody2D rb;
    private SpriteRenderer sRend; //variable to control the flip on movement
    [HideInInspector] public Animator anim;

    //Other game objects if needed
    public Animator switchAnim;
    public Animator chestAnim;
    public GameObject obstacle;
    public Animator switchAnim2;
    public GameObject obstacle2;
    public Animator switchAnim3;
    public GameObject obstacle3;
    public GameObject finalDoor;
    public GameObject crate;
    private Rigidbody2D craterb;
    public BoxCollider2D attackBox;
    private Boss boss;


    void Start()
    {
        attackBox = GetComponentInChildren<BoxCollider2D>();
        attackBox.enabled = false;
        canAttack = true;
        skeletonMode = false;
        isOrange = false;
        boss = FindObjectOfType<Boss>();

        rb = GetComponent<Rigidbody2D>();
        rb.sharedMaterial = sticky;
        anim = GetComponent<Animator>();
        sRend = GetComponent<SpriteRenderer>();
        playerAudio = GetComponent<AudioSource>();

        canControl = true;
        isDead = false;
        levelFinished = false;
        forcedContinue = false;
        bounce = false;
        jumpsLeft = maxJumps;
        inSight = false;
        onVerticalLift = false;
        onHorizontalLift = false;
        isMoving = true;
        invincible = false;
        exitTreasureRoom = false;
        treasureRoom = false;
        batTime = false;
        canPort = false;
        canReturn = false;
        checkPointReached = false;
        isRight = false;
        isLeft = false;
        finalDoorShowing = false;
        tut1 = false;
        tut2 = false;
        tut3 = false;

        

        //Find the switch/obstacle animator components, IF because errors without it
        if (GameObject.FindGameObjectWithTag("Crate"))
        {
            craterb = GameObject.FindGameObjectWithTag("Crate").GetComponent<Rigidbody2D>();
        }
        
        if (GameObject.FindGameObjectWithTag("Switch"))
        {
            switchAnim = GameObject.FindGameObjectWithTag("Switch").GetComponent<Animator>();
        }
        if (GameObject.FindGameObjectWithTag("Switch2"))
        {
            switchAnim2 = GameObject.FindGameObjectWithTag("Switch2").GetComponent<Animator>();
        }
        if (GameObject.FindGameObjectWithTag("Switch3"))
        {
            switchAnim3 = GameObject.FindGameObjectWithTag("Switch3").GetComponent<Animator>();
        }


        if (GameObject.FindGameObjectWithTag("Chest"))
        {
            chestAnim = GameObject.FindGameObjectWithTag("Chest").GetComponent<Animator>();
        }

        if (GameObject.Find("Obstacle"))
        {
            obstacle = GameObject.Find("Obstacle");
        }
        if (GameObject.Find("Obstacle2"))
        {
            obstacle2 = GameObject.Find("Obstacle2");
        }
        if (GameObject.Find("Obstacle3"))
        {
            obstacle3 = GameObject.Find("Obstacle3");
        }
    }

    void Update()
    {
        GroundCheck();
        wallCheck();
        treasureRoomEnter();
        houseEnter();

        float ySpeed = rb.velocity.y;

        if (invincible)
        {
            invTimer += Time.deltaTime;
            flicker();
        }

        if (isWalled == true)
        {
            rb.sharedMaterial = slippery;
        }

        if (isDead)
        {
            canControl = false;
            anim.SetBool("Death", true);
            if (!isGrounded)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else
            {
                rb.velocity = new Vector2(0f, 0f);
            }
        }

        if (canControl)
        {
            if (Input.GetMouseButtonDown(0) && skeletonMode && canAttack)
            {
                anim.SetTrigger("Attack");
                Invoke("attack", 0.2f);
                Invoke("cancelAttack", 0.4f);
            }

        //idling
        if (!Input.GetButton("Jump"))
            {
                isMoving = false; //made so i can jump on moving platforms
            }

            //moving X
            if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) //left
            {
                attackBox.transform.localScale = new Vector2(-0.8f, 1f);
                anim.SetBool("Moving", true);
                sRend.flipX = true;
                rb.velocity = new Vector2(-speed, ySpeed);
                isMoving = true;
            }
            else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) //right
            {
                attackBox.transform.localScale = new Vector2(0.8f, 1f);
                anim.SetBool("Moving", true);
                sRend.flipX = false;
                rb.velocity = new Vector2(speed, ySpeed);
                isMoving = true;
            }
            else // not moving
            {
                anim.SetBool("Moving", false);
                rb.velocity = new Vector2(0f, ySpeed);
            }


            //jumping Y
            if ((Input.GetButtonDown("Jump") && jumpsLeft != 0))
            {
                isMoving = true;
                if (isGrounded)
                {
                    playerAudio.PlayOneShot(jumpSound, 0.5f);
                    isMoving = true;
                    jumpsLeft -= 1;
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
                    anim.SetBool("Jumping", true);
                }

                if (jumpsLeft != 0 && Input.GetButtonDown("Jump") && !isGrounded)
                {
                    playerAudio.PlayOneShot(jumpSound, 0.5f);
                    isMoving = true;
                    anim.SetBool("Falling", false);
                    anim.SetBool("Jumping", true);
                    jumpsLeft -= 1;
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
                }
            }
            else if (isGrounded)
            {
                anim.SetBool("Jumping", false);
            }


            if (rb.velocity.y < 0 && !isGrounded) //Full jump descent
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                anim.SetBool("Falling", true);
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !isGrounded) //Half jump descent
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                anim.SetBool("Falling", true);
            }
            else if (isGrounded)
            {
                anim.SetBool("Falling", false);
            }

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Tutorial1":
                tut1 = true;
                break;

            case "Tutorial2":
                tut2 = true;
                break;

            case "Tutorial3":
                tut3 = true;
                break;

            case "RightTrigger":
                isRight = true;
                isLeft = false;
                boss.playerInSight = true;
                break;

            case "LeftTrigger":
                isLeft = true;
                isRight = false;
                boss.playerInSight = true;
                break;

            case "Checkpoint":
                checkPointReached = true;
                break;

            case "Level2Door":
                canPort = true;     
                break;

            case "Level2Door2":
                canReturn = true;
                break;

            case "Chain":
                playerAudio.PlayOneShot(switchSound, 0.5f);
                crate.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                break;

            case "BatTrigger":
                batTime = true;
                canControl = false;
                rb.velocity = new Vector2(0, 0);
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", false);
                anim.SetBool("Moving", false);
                anim.SetTrigger("Idle");
                break;

            case "FallTrigger":
                anim.ResetTrigger("Idle");
                anim.SetTrigger("FallTrigger");
                anim.SetBool("Falling", true);
                break;
                
            case "ExitRoom":
                exitTreasureRoom = true;
                break;

            case "NotDoor":
                treasureRoom = true;
                break;

            case "HealthPotion":
                playerAudio.PlayOneShot(powerupSound, 0.5f);
                UIController.playerHP = 4;
                Destroy(other.gameObject);
                break;

            case "Enemy":
                if (!invincible && attackBox.enabled == false && !isDead)
                {
                    playerAudio.PlayOneShot(recoilSound, 0.5f);
                    UIController.playerHP -= 1;
                    invincible = true;
                    Invoke("resetInvincibility", 2);
                }
                break;

            case "Boss":
                if (!invincible && !boss.stunned)
                {
                    Debug.Log(UIController.playerHP);
                    playerAudio.PlayOneShot(recoilSound, 0.5f);
                    invincible = true;
                    UIController.playerHP -= 1;
                    Invoke("resetInvincibility", 2);
                }
                break;

            case "Spikes":
                if (UIController.playerHP == 4)
                {
                    playerAudio.PlayOneShot(spikeSound, 0.5f);
                    UIController.playerHP -= 1;
                    invincible = true;
                    Invoke("resetInvincibility", 2);
                    break;
                } 
                else
                {
                    playerAudio.PlayOneShot(spikeSound, 0.5f);
                    canControl = false;
                    UIController.playerHP = 0;
                    isDead = true;
                    break;
                }
                

            case "Finish":
                canControl = false;
                anim.SetBool("Moving", false);
                rb.velocity = new Vector2(0f, 0f);
                levelFinished = true;
                break;

            case "ForcedContinue":
                canControl = false;
                anim.SetBool("Moving", false);
                forcedContinue = true;
                break;

            case "Switch":
                switchAnim.SetBool("Pressed", true);   //switches that activate obstacles
                obstacle.GetComponent<ObstacleMover>().switchActivated = true;
                break;

            case "Switch2":
                switchAnim2.SetBool("Pressed", true);
                obstacle2.GetComponent<ObstacleMover>().switch2Activated = true;
                break;

            case "Switch3":
                switchAnim3.SetBool("Pressed", true);
                obstacle3.GetComponent<ObstacleMover>().switch3Activated = true;
                break;

            case "Elevator":    //if player hits elevator
                Elevator elevator = other.gameObject.GetComponent<Elevator>(); // Get the elevator script
                if (elevator.isHorizontal)
                {
                    onHorizontalLift = true;
                }
                else
                {
                    onVerticalLift = true;
                }
                break;

            case "PickUp": //if player hits a pickup skull for HP
                Destroy(other.gameObject);
                playerAudio.PlayOneShot(powerupSound, 0.5f);
                if (UIController.playerHP < 3)
                {
                    UIController.playerHP += 1;
                }
                break;

            case "Coin":
                playerAudio.PlayOneShot(pickupSound, 0.5f);
                UIController.score += other.gameObject.GetComponent<Coin>().points;
                break;

            case "SpeedPotion":
                playerAudio.PlayOneShot(powerupSound, 0.5f);
                Destroy(other.gameObject);
                changeOrange();
                speed += 3;
                Invoke("resetColour", 6);
                Invoke("resetSpeed", 6);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Tutorial1":
                tut1 = false;
                break;

            case "Tutorial2":
                tut2 = false;
                break;

            case "Tutorial3":
                tut3 = false;
                break;

            case "Elevator":    //when player exits the elevator
                onHorizontalLift = false;
                onVerticalLift = false;
                break;

            case "NotDoor":
                treasureRoom = false;
                break;

            case "ExitRoom":
                exitTreasureRoom = false;
                break;
            
            case "Level2Door":
                canPort = false;
                break;

            case "Level2Door2":
                canReturn = false;
                break;
        }

    }

    void resetIdle()
    {
        anim.ResetTrigger("Idle");
    }

    void playerFall()
    {
        anim.SetBool("Falling", true);
    }

    public void treasureRoomEnter()
    {
        if (treasureRoom && Input.GetKeyDown(KeyCode.W)) 
        {
            transform.position = new Vector3(58.72f, -36.06f, 0);
            treasureRoom = false;
        }
        if (exitTreasureRoom && Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(-2.7f, -14.6f, 0);
            exitTreasureRoom = false;
        }
    }

    public void houseEnter() //change player pos in town
    {
        if (canPort && Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(27.89f, -24.09f, 0);
            canPort = false;
        }
        if (canReturn && Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(32.96f, 1.89f, 0);
            canReturn = false;
        }
    }


void flicker() //invincibilty timer with potion effects 
    {
        if (invTimer < 2f && !isOrange)
        {
            if (invTimer > 0 && invTimer < 0.5f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            }
            else if (invTimer > 0.5f && invTimer < 1f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            else if (invTimer > 1f && invTimer < 1.5f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            }
            else if (invTimer > 1.5f && invTimer < 1.8f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                invincible = false;
                invTimer = 0;
            }
        } 
        else if (invTimer < 2f && isOrange)
        {
            if (invTimer > 0 && invTimer < 0.5f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 0.64f, 0, 0.2f);
            }
            else if (invTimer > 0.5f && invTimer < 1f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 0.64f, 0, 1);
            }
            else if (invTimer > 1f && invTimer < 1.5f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 0.64f, 0, 0.2f);
            }
            else if (invTimer > 1.5f && invTimer < 1.8f)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 0.64f, 0, 0.6f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 0.64f, 0, 1f);
                invincible = false;
                invTimer = 0;
            }
        }
    }

    void changeOrange()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0.64f, 0, 1);
        isOrange = true;
    }

    void resetColour()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        isOrange = false;
    }


    void wallCheck()
    {
        isWalled = false;

        Collider2D[] wallColliders = Physics2D.OverlapCircleAll(wallCheckCollider.position, wallCheckRadius, groundLayer);
        if (wallColliders.Length > 0)
        {
            isWalled = true;
        }
    }

    void resetInvincibility()
    {
        invincible = false;
    }

    void resetSpeed()
    {
        speed = 6f;
    }

    void attack()
    {
        canAttack = false;
        attackTimer += Time.deltaTime;
        playerAudio.PlayOneShot(swingSound, 0.5f);
        attackBox.enabled = true;
    }

    void cancelAttack()
    {
        canAttack = true;
        attackBox.enabled = false;
        attackTimer = 0f;
    }

    void GroundCheck()
    {
        isGrounded = false;

        //Check if the GroundCheckObject is collididing with other 2D colliders that are in the "Ground" layer, true if isGrounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            jumpsLeft = maxJumps;
        }
    }

    public void slimeBounce()
    {
        playerAudio.PlayOneShot(slimeSound, 0.5f);
        rb.velocity = new Vector2(0f, 10f);   
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //if player hits a projectile
        if (other.gameObject.tag == "Projectile")
        {
            if (!invincible && !isDead)
            {
                playerAudio.PlayOneShot(recoilSound, 0.5f);
                UIController.playerHP -= 1;
                invincible = true;
                Invoke("resetInvincibility", 2);
            }
            
        }
    }
    
}
