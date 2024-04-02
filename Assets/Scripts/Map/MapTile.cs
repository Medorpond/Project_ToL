using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    grass, wall, block,
}

public class MapTile : MonoBehaviour
{
    public TileType tileType = TileType.grass;

    // Tile의 Sprite
    [SerializeField]
    private SpriteRenderer tileRenderer;
    [SerializeField]
    private Sprite[] tileSprites;
    [SerializeField]
    private Sprite[] grassTileSprites;

    [SerializeField]
    private GameObject highlight;



    public void SettingTile(TileType type)
    {
        SetTileSprite();
    }

    private void SetTileSprite()
    {
        // Tile Sprite 설정
        if (tileType == TileType.grass) SetGrassTileSprite();
        else tileRenderer.sprite = tileSprites[(int)tileType];
    }

    private void SetGrassTileSprite()
    {
        // 기본 Grass Sprite 랜덤으로 생성
        tileRenderer.sprite = grassTileSprites[Random.Range(0, grassTileSprites.Length)];
    }

    private void OnMouseEnter()
    {
        if (tileType == TileType.grass)
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
