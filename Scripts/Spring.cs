using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float springForce = 15f;

    private Animator anim;
    private Player player;
    private AudioSource audioplayer;
    public AudioClip springSound;

    // Start is called before the first frame update
    void Start()
    {
        audioplayer = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.rb.velocity = new Vector2(0f, springForce);
            audioplayer.PlayOneShot(springSound, 0.5f);
            anim.SetTrigger("PlayerTouch");
        }
    }
}
