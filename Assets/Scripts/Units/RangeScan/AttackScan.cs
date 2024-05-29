using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScan : MonoBehaviour
{
    public List<GameObject> inRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit"))
        {
            // Add the Object of collision
        }
    }
}
