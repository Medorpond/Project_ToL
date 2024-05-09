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

        // ���콺 �������� ���� ��ġ�� ������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���� ī�޶��� ��ġ�� ���콺 ������ ������ �Ÿ� ���
        float distanceToMouse = Vector2.Distance(transform.position, mousePos);

        // ī�޶� ���콺 �����ͷ� �̵��ϰ� �� ��/�ƿ�
        transform.position = Vector3.MoveTowards(transform.position, mousePos, scroll * zoomSpeed * distanceToMouse);
        Camera.main.orthographicSize = newSize;
    }
}
