using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharSelect : MonoBehaviour
{   
    // for Prefab
    
    [SerializeField]
    private GameObject[] characters;
    [SerializeField]
    private GameObject[] CharBtn; // prefabs
    [SerializeField]
    private GameObject DoneBtn; // done button
    [SerializeField]
    private GameObject ResetBtn; // reset button
    [SerializeField]
    private GameObject createtextPrefab;

    //
    Vector3 createPoint = new Vector3(0, 100, 0); // first spawn
    private int clickCount = 0; // 3 ~ 7 char count
    //
    // for destroy
    List<GameObject> setButtons = new List<GameObject>(); 
    private GameObject instantiatedDoneBtn; 
    private GameObject ResetButton; 
    
    private GameObject CreatecountObj;
    private TextMeshProUGUI createCountText;
    
    // for destroy
    

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

        // Reset Button Instantiate
        Button ResetButtonclicked = ResetButton.GetComponent<Button>();
        if (ResetButtonclicked != null)
        {
            ResetButtonclicked.onClick.AddListener(ResetClick);
        }
        
        // text Instantiate
        createPoint = new Vector3 (660, 100, 0);
        CreatecountObj = Instantiate(createtextPrefab, Vector3.zero, Quaternion.identity, parentTransform);
        CreatecountObj.transform.localPosition = createPoint;
        createCountText = CreatecountObj.GetComponent<TextMeshProUGUI>();
    }

    void OnButtonClick(int index){ // click 1-level button(choose)
        player.SetCharacter(characters[index]);

        clickCount++;      // created 2-level button count
        UpdateCreateCountText();
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
            Destroy(CreatecountObj);
        }
    }

    void UpdateCreateCountText()
    {
        if (createCountText != null)
        {
            createCountText.text = "Create : " + clickCount.ToString();
        }
    }
    void ResetClick()
    {
    }
}

