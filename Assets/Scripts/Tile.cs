using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ReactOnClick
{
    [SerializeField]
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if(gameManager.UnitSelected != null)
        {
            gameManager.UnitSelected.transform.position = transform.position;
            gameManager.UnitSelected = null;
        }
        
    }
}
