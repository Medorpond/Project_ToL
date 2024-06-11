using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    [SerializeField]
    private Button Movebutton;
    [SerializeField]
    private Button Attackbutton;

    public Color selectedColor;
    private Color originalColor;

    private void Start()
    {
        originalColor = Movebutton.colors.normalColor;
    }

    public void MoveColor()
    {

    }
    
    public void RestButtonColor()
    {
    
    }
}
