using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characters;
    [SerializeField]
    private GameObject[] CharBtn; // prefabs
    [SerializeField]
    private GameObject DoneBtn; // done button
    [SerializeField]
    private GameObject ResetBtn; // reset button
    Vector3 createPoint = new Vector3(0, 100, 0); // first spawn
    private int x = 0; // + 100
    private int clickCount = 0; // 3 ~ 7 char count
    List<GameObject> setButtons = new List<GameObject>(); // for destroy
    List<GameObject> createdBtns = new List<GameObject>(); // for reference
    GameObject instantiatedDoneBtn; // for destroy
    GameObject ResetButton;
    
    [SerializeField]
    private Player player;
    void Start()
    {
        Transform parentTransform = GameObject.Find("SelectChar").transform; // sub SelectChar
        for(int i = 0; i < CharBtn.Length; i++) // create 1 level button(choose button)
        {
            GameObject instantiatedBtn = Instantiate(CharBtn[i], Vector3.zero, Quaternion.identity, parentTransform); // create 0, 0, 0 
            instantiatedBtn.transform.localPosition = createPoint; // move to createPoint
            createPoint.x += 100; // next + 100
            setButtons.Add(instantiatedBtn); // setButtons add

            Button button = instantiatedBtn.GetComponent<Button>(); // button click
            if (button != null) 
            {
                int index = i; // ex) 1 = tanker, 2 = dealer, ...
                button.onClick.AddListener(() => OnButtonClick(index));
            }
        }
        createPoint = new Vector3(710, 175, 0); // right-top
        instantiatedDoneBtn = Instantiate(DoneBtn, Vector3.zero, Quaternion.identity, parentTransform); // create end button
        instantiatedDoneBtn.transform.localPosition = createPoint; // move to craetePoint

        Button Donebutton = instantiatedDoneBtn.GetComponent<Button>(); // end button click
        if (Donebutton != null)
        {
            Donebutton.onClick.AddListener(onDoneButtonClick);
        }

        createPoint = new Vector3(710, 25, 0);
        ResetButton = Instantiate(ResetBtn, Vector3.zero, Quaternion.identity, parentTransform);
        ResetButton.transform.localPosition = createPoint;

        Button ResetButtonclicked = ResetButton.GetComponent<Button>();
        if (ResetButtonclicked != null)
        {
            ResetButtonclicked.onClick.AddListener(ResetClick);
        }
    }

    void OnButtonClick(int index){ // click 1-level button(choose)
        player.SetCharacter(characters[index]);

        clickCount++;      // created 2-level button count
        if (clickCount >= 7)    // why 7? size issue
        {
            onDoneButtonClick(); // auto end
        }

    }

    void onDoneButtonClick(){ // click end 
        if (clickCount >= 3 ){      // created at least 3
            Destroy(instantiatedDoneBtn); // destroy end button
            foreach(GameObject btn in setButtons) // destroy 1-level button
            {
                Destroy(btn);
            }
            Destroy(ResetButton);
        }
    }
    void ResetClick()
    {
    }
}
