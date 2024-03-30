using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloud : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    void Update()
    {
        transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
        if(transform.position.x < -20)
        {
            transform.position += new Vector3(40, 0, 0);
        }
    }
}
