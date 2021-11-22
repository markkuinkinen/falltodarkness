using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Boss boss;

    void Start()
    {
        boss = FindObjectOfType<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform bar = transform.Find("Bar");
        if (boss.bossHP == 9)
        {
            bar.localScale = new Vector3(0.9f, 1f);
        }
        else if (boss.bossHP == 8)
        {
            bar.localScale = new Vector3(0.8f, 1f);
        }
        else if (boss.bossHP == 7)
        {
            bar.localScale = new Vector3(0.7f, 1f);
        }
        else if (boss.bossHP == 6)
        {
            bar.localScale = new Vector3(0.6f, 1f);
        }
        else if (boss.bossHP == 5)
        {
            bar.localScale = new Vector3(0.5f, 1f);
        }
        else if (boss.bossHP == 4)
        {
            bar.localScale = new Vector3(0.4f, 1);
        }
        else if (boss.bossHP == 3)
        {
            bar.localScale = new Vector3(0.3f, 1f);
        }
        else if (boss.bossHP == 2)
        {
            bar.localScale = new Vector3(0.2f, 1f);
        }
        else if (boss.bossHP == 1)
        {
            bar.localScale = new Vector3(0.1f, 1f);
        }
        else if (boss.bossHP == 0)
        {
            bar.localScale = new Vector3(0f, 0f);
            boss.isDead = true;
            Destroy(gameObject, 2f);
        }
    }
}
