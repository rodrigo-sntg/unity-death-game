using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourScript : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private float xMax;
    [SerializeField]
    private float yMax;
    [SerializeField]
    private float xMin;
    [SerializeField]
    private float yMin;
     
     void Start () {
         player = GameObject.Find ("Hero").transform;
     }
 
     void Update () {
         transform.position = new Vector3 (Mathf.Clamp(player.position.x,xMin,xMax), Mathf.Clamp(player.position.y,yMin,yMax), player.position.z - 10);
         //transform.position = new Vector3 (player.position.x, player.position.y + 5, player.position.z - 10);
     }
}