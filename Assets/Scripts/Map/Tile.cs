using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private GameObject highlight;

    private void Update()
    {
        if (transform.position.x - 0.5 < Camera.main.ScreenToWorldPoint(Input.mousePosition).x &&
            transform.position.x + 0.5 > Camera.main.ScreenToWorldPoint(Input.mousePosition).x &&
            transform.position.y + 0.5 > Camera.main.ScreenToWorldPoint(Input.mousePosition).y &&
            transform.position.y - 0.5 < Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
        {
            highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
        }
    }
}
