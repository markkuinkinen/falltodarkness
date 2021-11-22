using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private float destroyTime = 0.01f;
    private UIController ui;

    public int points; //the amount of points rewarded to the player, changed in inspector ~100, given in player script


    void Start()
    {
        ui = GetComponent<UIController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject, destroyTime);
        }
    }
}
