using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D doorTrigger;
    private Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = FindObjectOfType<Boss>();
        doorTrigger = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        doorTrigger.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.isDead)
        {
            anim.SetTrigger("Open");
            doorTrigger.enabled = true;
        }
    }
}
