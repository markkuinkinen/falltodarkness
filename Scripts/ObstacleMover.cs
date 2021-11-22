using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private float timer;

    [HideInInspector] public bool switchActivated;
    [HideInInspector] public bool switch2Activated;
    [HideInInspector] public bool switch3Activated;

    public Rigidbody2D rb;
    public Rigidbody2D rb2;
    public Rigidbody2D rb3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb2 = (GameObject.Find("Obstacle2")).GetComponent<Rigidbody2D>();
        rb3 = (GameObject.Find("Obstacle3")).GetComponent<Rigidbody2D>();

        timer = 0f;
    }

    void FixedUpdate()
    {
        if (switchActivated)
        {
            timer += Time.deltaTime;

            if (timer < 6.3f)
            {
                rb.velocity = new Vector2(0f, 1f);
            }
            else
            {
                Destroy(gameObject, 1);
                rb.velocity = new Vector2(0f, 0f);
                switchActivated = false;
                timer = 0f;
            }
        }

        if (switch2Activated)
        {
            timer += Time.deltaTime; 

            if (timer < 6.3f)
            {
                rb2.velocity = new Vector2(0f, -1f);;
            }
            else
            {
                Destroy(gameObject, 1);
                rb2.velocity = new Vector2(0f, 0f);
                switch2Activated = false;
                timer = 0f;
            }
        }
        
        if (switch3Activated)
        {
            timer += Time.deltaTime;

            if (timer < 6.3f)
            {
                rb3.velocity = new Vector2(1.5f, 0f);
            }
            else
            {
                Destroy(gameObject, 1);
                rb3.velocity = new Vector2(0f, 0f);
                switch3Activated = false;
                timer = 0f;
            }
        }
    
    }
}
