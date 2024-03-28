using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // Detect left mouse button click
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit.collider != null)
            {
                ReactOnClick react = hit.collider.gameObject.GetComponent<ReactOnClick>();
                if (react != null)
                {
                    react.OnClick();
                }
            }
        }
    }
}
