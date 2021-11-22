using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public float timer;
    public bool isHorizontal;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime; 

        if (timer < 1f)
        {
            if (isHorizontal)
            {
                rb.velocity = new Vector2(4f, 0f);
            }
            else
            {
                rb.velocity = new Vector2(0f, -4f); //( x and y )
            }

        }
        else if (timer > 1 && timer < 3f) 
        {
            if (isHorizontal)
            {
                rb.velocity = new Vector2(-1.98f, 0f);
            }
            else
            {
                rb.velocity = new Vector2(0f, 1.98f);
            }

        }
        else
        {
            timer = 0f;
        }

    }
}
