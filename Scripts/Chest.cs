using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    float invokeTime = 0.5f;

    public bool isOpen;
    private Animator anim;

    int randomNumber;

    public GameObject coin;
    public GameObject potion;
    public GameObject slime;

    public Transform itemSpawn;
    public Transform slimeSpawn;

    void Start()
    {
        isOpen = false;
        anim = GetComponent<Animator>();       
        randomNumber = Random.Range(1, 4); //random number between 1-3 for when chest is activated, determining the contents 
    }

    void Update()
    {
        if (isOpen == true)
        {
            anim.SetTrigger("ChestOpen");
            Invoke("randomContents", invokeTime);
            Debug.Log(randomNumber);
        }
    }

    void randomContents()
    {
        if (randomNumber == 1)
        {
            GameObject coinClone;
            coinClone = Instantiate(coin, itemSpawn.position, itemSpawn.rotation);
            isOpen = false;
            randomNumber = 0;
        } 
        else if (randomNumber == 2)
        {
            GameObject potionClone;
            potionClone = Instantiate(potion, itemSpawn.position, itemSpawn.rotation);
            isOpen = false;
            randomNumber = 0;
        } 
        else if (randomNumber == 3)
        {
            GameObject slimeClone;
            slimeClone = Instantiate(slime, slimeSpawn.position, slimeSpawn.rotation);
            isOpen = false;
            randomNumber = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOpen = true;
        }
    }
}
