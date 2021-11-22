using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timer;
    private bool flicker;
    private Rigidbody2D rb;
    private Player player;

    void Start()
    {
        flicker = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flicker)
        {
            flickerDeathTimer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            flicker = true;
            Invoke("slowDestroy", 2);
            Invoke("restorePlatform", 6);
        }
    }

    void slowDestroy()
    {
        gameObject.SetActive(false);
    }

    void restorePlatform()
    {
        gameObject.SetActive(true);
    }

    void flickerDeathTimer()
    {
        timer += Time.deltaTime;

        if (timer > 0 && timer < 0.5f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            rb.velocity = new Vector2(0, -0.7f);
            
        } 
        else if (timer > 0.5f && timer < 1) 
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            rb.velocity = new Vector2(0, 0.7f);
        } 
        else if (timer > 1 && timer < 1.5f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            rb.velocity = new Vector2(0, -0.7f);
        }
        else if (timer > 1.5f && timer < 2)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            rb.velocity = new Vector2(0, 0.7f);
        } else
        {
            flicker = false;
            timer = 0;
        }
    }
}
