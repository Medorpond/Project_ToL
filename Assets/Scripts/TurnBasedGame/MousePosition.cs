using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 nodePosition;

    [SerializeField]
    private GridManager gridManager;


    
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        }
    }

    public bool CheckRange()
    {
        if (mousePosition.x < -0.5 || mousePosition.y < -0.5 || mousePosition.x > 0.5 + gridManager.width || mousePosition.y > 0.5 + gridManager.height) return false;

        return true;
    }

    public Vector3 SwitchToNode()
    {
        nodePosition = new Vector3((float)System.Math.Truncate(mousePosition.x + 0.5f), (float)System.Math.Truncate(mousePosition.y + 0.5f), 0);
        return nodePosition;
    }
}
