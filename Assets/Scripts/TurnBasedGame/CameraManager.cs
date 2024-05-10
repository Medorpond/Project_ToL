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

    [SerializeField]
    private AnimationCurve moveCurve;
    private Vector3 targetPosition;
    private float cameraMoveTime = 0.1f;



    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            targetPosition.z = transform.position.z;

            StartCoroutine("MoveCamera");
        }

        // zoom inout with scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Camera.main.orthographicSize > maxZoom && scroll < 0) return;
        if (Camera.main.orthographicSize < minZoom && scroll > 0) return;
        else Zoom(scroll);
    }

    private IEnumerator MoveCamera()
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / cameraMoveTime;

            transform.position = Vector3.Lerp(transform.position, targetPosition, moveCurve.Evaluate(percent));

            yield return null;
        }
    }

    private void Zoom(float scroll)
    {
        float newSize = Camera.main.orthographicSize - scroll * zoomSpeed;
        Camera.main.orthographicSize = newSize;
    }
}
