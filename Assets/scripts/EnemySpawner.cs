using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private float randomX;

    private int numberOfEnemies = 0;
    private Vector2 placeToSpawn;
    public float spawnRate = 2f;

    public int maxEnemies = 1;
    private float nextSpawn = 0f;

    [SerializeField]
    private float minX;

    [SerializeField]
    private float maxX;

    private List<Collider2D> collidersList = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            if (numberOfEnemies <= maxEnemies)
            {
                nextSpawn = Time.time + spawnRate;
                randomX = Random.Range(minX, maxX);
                placeToSpawn = new Vector2(randomX, transform.position.y);
                numberOfEnemies++;

                GameObject obj = Instantiate(enemy, placeToSpawn, Quaternion.identity) as GameObject;
                obj.GetComponent<Collider2D>().enabled = false;
                obj.GetComponent<Collider2D>().enabled = true;

                IgnoreCollision ignore = obj.GetComponent<IgnoreCollision>();
                ignore.others.Add(obj.GetComponent<Collider2D>());
                ignore.other = Hero.Instance.GetComponent<Collider2D>();
                ignore.ignoreCollision();

                Collider2D colliderChild = obj.GetComponentInChildren<Collider2D>();
                GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");

                foreach (GameObject edge in edges)
                {
                    if (edge.name.ToLower().Contains("left"))
                        obj.GetComponent<Enemy>().leftEdge = edge.GetComponent<Transform>();
                    if (edge.name.ToLower().Contains("right"))
                        obj.GetComponent<Enemy>().rightEdge = edge.GetComponent<Transform>();
                }

                colliderChild.enabled = false;
                colliderChild.enabled = true;

            }
        }
    }
}