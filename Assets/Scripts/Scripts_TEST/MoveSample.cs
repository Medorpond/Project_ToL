using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class MoveSample : MonoBehaviour
{
    public Button button;
    public SwordMan unit;
    private Vector2Int origin = new Vector2Int(6, 8);
    private Vector2Int destination = new Vector2Int(25, 10);

    private void Start()
    {
        button.onClick.AddListener(Action);
    }
    // Update is called once per frame
    void Action()
    {
        if (unit.transform.position.x != 6 || unit.transform.position.y != 8) { StartCoroutine(unit.Move(origin));}
        else if (unit.transform.position.x != 25 || unit.transform.position.y != 10) { StartCoroutine(unit.Move(destination)); }
        else { return; }
    }
}
