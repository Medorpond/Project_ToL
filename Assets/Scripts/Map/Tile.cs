using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private GameObject highlight;
    private GameObject player;

    private void Awake()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        player = GameObject.FindWithTag("Player");
    }

    private void OnMouseEnter() => highlight.SetActive(true);
    void OnMouseUp()
    {
        if (player != null)
        {
            player.SendMessage("ReceiveClicked", gameObject);
        }
    }

    private void OnMouseExit() => highlight.SetActive(false);

}
