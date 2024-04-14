using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MoveSample : MonoBehaviour
{
    public Button button;
    public SwordMan unit;
    private Vector2Int origin = new Vector2Int(1, 0);
    private Vector2Int destination = new Vector2Int(4, 0);

    private void Start()
    {
        button.onClick.AddListener(Action);
    }
    // Update is called once per frame
    void Action()
    {
        if (unit.transform.position.x != 1) { StartCoroutine(unit.Move(origin)); }
        else if (unit.transform.position.x != 4) { StartCoroutine(unit.Move(destination)); }
        else { return; }
    }
}
