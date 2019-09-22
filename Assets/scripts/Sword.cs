using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Sword : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D sword;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        sword = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    public void Init(Vector2 direction)
    {
        this.direction = direction;
    }

    void FixedUpdate()
    {
        sword.velocity = direction * speed;
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}