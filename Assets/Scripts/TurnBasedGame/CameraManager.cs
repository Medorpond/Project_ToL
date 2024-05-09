using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float zoomSpeed = 3.0f;
    [SerializeField]
    private float minZoom = 1.0f;
    [SerializeField]
    private float maxZoom = 10.0f;
    private float cameraMovespeed = 5.0f;



    private void Update()
    {
        
        if (Input.GetMouseButtonDown(1)) MoveMap();
   
        // zoom inout with scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Camera.main.orthographicSize > maxZoom && scroll < 0) return;
        if (Camera.main.orthographicSize < minZoom && scroll > 0) return;
        else Zoom(scroll);
    }

    private void MoveMap()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
        transform.position = targetPosition;
    }

    private void Zoom(float scroll)
    {
        float newSize = Camera.main.orthographicSize - scroll * zoomSpeed;
        Camera.main.orthographicSize = newSize;
    }
}
