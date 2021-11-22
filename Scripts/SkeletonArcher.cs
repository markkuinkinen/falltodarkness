using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : MonoBehaviour
{
    public float fireFrequency = 1.29f; //interval for shooting
    public float fireTimer;
    public float animTimer;
    private float projectileSpeed = 5f;
    
    private Player player;
    private CapsuleCollider2D hitBox;
    private LineOfSight lineOfSight;
    private Animator anim;
    private BoxCollider2D canSee;
    public GameObject projectile;
    public Transform projectileSpawn;

    public bool playerInSight;
    public bool animCheck;
    public bool facingLeft;
    public bool isDead;
    public bool inSight;

    public AudioSource skeletonAudio;
    public AudioClip deathSound;
    

    void Start()
    {
        skeletonAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        lineOfSight = FindObjectOfType<LineOfSight>();
        hitBox = GetComponent<CapsuleCollider2D>();
        canSee = GetComponentInChildren<BoxCollider2D>();

        animCheck = false;
        isDead = false;
        inSight = false;

        if (facingLeft)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f); //flips whole gameObject and its children
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (inSight)
            {
                anim.SetBool("ShootBool", true);
                constantShoot();
            }
            else
            {   
                fireTimer = 0;
                anim.SetBool("ShootBool", false);
            }
        } else
        {
            isDead = true;
            hitBox.enabled = false;
            anim.SetBool("SkeletonDeath", true);
            canSee.enabled = false;
        }

    }


    void inSightCheck()
    {
        if (inSight)
        {
            anim.SetBool("ShootBool", true);
            Invoke("constantShoot", 1.3f);
            
        }
        else
        {
            fireTimer = 0f;
            playerInSight = false;
            anim.SetBool("ShootBool", false);
            anim.SetTrigger("Shooting");
        }
    }

    void animationTimer()
    {
        animTimer += Time.deltaTime;
    }

    void animationCheck()
    {
        GameObject projectileClone;
        animationTimer();
        
        if (animTimer > fireFrequency)
        {
            if (facingLeft)
            {
                projectileClone = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
                projectileClone.transform.localScale = new Vector3(-1f, 1f, 1f);
                projectileClone.GetComponent<Rigidbody2D>().velocity = transform.right * -projectileSpeed;
                animCheck = false;
                animTimer = 0f;
            } else
            {

                fireTimer = 0f;
                projectileClone = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
                projectileClone.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
                animCheck = false;
                animTimer = 0f;
            }
        }
    }


    void constantShoot()
    {
        GameObject projectileClone;
        fireTimer += Time.deltaTime;

        if (fireTimer > fireFrequency)
        {
            if (facingLeft)
            {
                projectileClone = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
                projectileClone.transform.localScale = new Vector3(-1f, 1f, 1f); //flip projectile to correct direction
                projectileClone.GetComponent<Rigidbody2D>().velocity = transform.right * -projectileSpeed; //move left, neg x axis
                fireTimer = 0f;
            }
            else
            {
                projectileClone = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
                projectileClone.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
                fireTimer = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "AttackBox")
        {
            isDead = true;
            skeletonAudio.PlayOneShot(deathSound, 0.5f);
        }

    }

}
