using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    idle = 0, obstacle
}

public class MapTile : MonoBehaviour
{
    public TileType tileType = TileType.idle;

    [SerializeField]
    private SpriteRenderer tileRenderer;

    [SerializeField]
    private GameObject highlight;



    public void SetTileColor(bool chooseColor)
    {
        if (chooseColor) tileRenderer.color = new Color(0.164f, 0.271f, 0.274f);
        else tileRenderer.color = new Color(0.164f, 0.204f, 0.274f);
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
