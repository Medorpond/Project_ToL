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
        float newSize = Camera.main.orthographicSize - scroll * zoomSpeed;
        newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

        // 마우스 포인터의 현재 위치를 가져옴
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 현재 카메라의 위치와 마우스 포인터 사이의 거리 계산
        float distanceToMouse = Vector2.Distance(transform.position, mousePos);

        // 카메라를 마우스 포인터로 이동하고 줌 인/아웃
        transform.position = Vector3.MoveTowards(transform.position, mousePos, scroll * zoomSpeed * distanceToMouse);
        Camera.main.orthographicSize = newSize;
    }
}
