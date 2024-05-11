using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera cam;
    private Vector3 firstPosition;
    
    [SerializeField]
    private float zoomSpeed = 3.0f;
    [SerializeField]
    private float minZoom = 1.0f;
    [SerializeField]
    private float maxZoom = 10.0f;

    private Vector3 difference;
    private Vector3 dragOrigin;



    private void Awake()
    {
        cam = Camera.main;
        firstPosition = cam.transform.position;
    }

    private void LateUpdate()
    {
        // move Camera
        if (Input.GetMouseButtonDown(1)) dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(1))
        {
            difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            if (CheckRange()) cam.transform.position += difference;
        }

        // zoom inout with scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (cam.orthographicSize > maxZoom && scroll < 0) return;
        if (cam.orthographicSize < minZoom && scroll > 0) return;
        else Zoom(scroll);
    }

    private void Zoom(float scroll)
    {
        float newSize = cam.orthographicSize - scroll * zoomSpeed;
        cam.orthographicSize = newSize;
    }

    public void ResetCamera()
    {
        cam.transform.position = firstPosition;
        cam.orthographicSize = maxZoom;
    }

    public void ZoomCharacter(Vector3 position)
    {
        cam.orthographicSize = 3.0f;
        cam.transform.position = position + new Vector3 (0, 1, -10);
    }

    // make range of camera move
    private bool CheckRange()
    {
        if (cam.transform.position.x < CalculateLeftRate() && difference.x < 0) return false;
        if (cam.transform.position.x > CalcuateRightRate() && difference.x > 0) return false;
        if (cam.transform.position.y > CalcuateUpRate() && difference.y > 0) return false;
        if (cam.transform.position.y < CalcuateDownRate() && difference.y < 0) return false;
        else return true;
    }

    private float CalculateLeftRate()
    {
        //y = (-11/9)x - 11/9
        return -(11 / 9) * (cam.orthographicSize + 1);
    }

    private float CalcuateRightRate()
    {
        //?y = x +27
        return cam.orthographicSize + 27.0f;
    }
    
    private float CalcuateUpRate()
    {
        //y = 5.5/9 x  + 93.5/9
        return (5.5f * cam.orthographicSize + 93.5f) / 9;
    }

    private float CalcuateDownRate()
    {
        //y = -2/3x - 5/6
        return -(2 * cam.orthographicSize + 2.5f) / 3;
    }
}
