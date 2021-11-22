using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public bool inSightTest;

    public BoxCollider2D hitBox;
    private SkeletonArcher skeletonArcher;
    
    void Start()
    {
        hitBox = GetComponent<BoxCollider2D>();
        GameObject skeletonArcherObject = transform.parent.gameObject;
        skeletonArcher = skeletonArcherObject.GetComponent<SkeletonArcher>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            skeletonArcher.inSight = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            skeletonArcher.inSight = false;
        }
    }
}
