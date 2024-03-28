using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject unitSelected;
    public GameObject UnitSelected
    {
        get { return unitSelected; }
        set { unitSelected = value; }
    }
}
