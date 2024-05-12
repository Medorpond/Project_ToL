using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; // Speed of the camera movement
    public float zoomSpeed = 1f; // Speed of the zoom
    public float minZoom = 1f; // Minimum zoom level (smallest orthographic size)
    public float maxZoom = 5f; // Maximum zoom level (largest orthographic size)

    public float minX = 2f; // Minimum x position of the camera
    public float maxX = 24f; // Maximum x position of the camera
    public float minY = 2f; // Minimum y position of the camera
    public float maxY = 9f; // Maximum y position of the camera

    private Vector3 dragOrigin; // Origin position of the cursor when dragging starts

    void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampCameraPosition();
    }

    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the position of the mouse in world space
        }

        if (Input.GetMouseButton(1)) // Right mouse button held down
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition); // Calculate the movement vector
            transform.position += difference; // Move the camera
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
    }

    void ClampCameraPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
