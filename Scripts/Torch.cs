using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Animator anim;
    public Boss boss;

    void Start()
    {
        anim = GetComponent<Animator>();
        boss = FindObjectOfType<Boss>();
    }

    void Update()
    {
        if (boss.isDead)
        {
            anim.SetTrigger("TorchOn"); //only used in boss fight
        }
    }
}
