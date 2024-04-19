using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMove : MonoBehaviour
{
    public int x;
    public int y;
    public float moveSpeed;
    Vector3 destination;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destination = new Vector3(x, y);
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed);
    }
}
