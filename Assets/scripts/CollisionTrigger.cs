using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    private BoxCollider2D playerCollider;

    [SerializeField]
    private BoxCollider2D platformCollider;

    [SerializeField]
    private BoxCollider2D platformTrigger;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.Find("Hero").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Hero")
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Hero")
        {
             Physics2D.IgnoreCollision(platformCollider, playerCollider, false);

        }
    }


}