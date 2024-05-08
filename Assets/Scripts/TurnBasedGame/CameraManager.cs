using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float zoomSpeed = 1.0f;
    private float minZoom = 1.0f;
    private float maxZoom = 10.0f;



    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
    }
}
