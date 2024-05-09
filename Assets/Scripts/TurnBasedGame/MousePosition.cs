using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private Vector3 mousePosition;

    

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        }
    }

    public Vector3 SwitchToNode()
    {
        return mousePosition;
    }
}
