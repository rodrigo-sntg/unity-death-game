using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField]
    public Collider2D other;

    [SerializeField]
    public List<Collider2D> others = new List<Collider2D>();

    private void Awake() => ignoreCollision();

    public void ignoreCollision()
    {
        Collider2D collider = GetComponent<Collider2D>();

        foreach (Collider2D item in others)
        {
            Physics2D.IgnoreCollision(collider, item);
        }

        Physics2D.IgnoreCollision(collider, other);
    }

    void Update()
    {
        if (other != null)
        {

        }

    }

}