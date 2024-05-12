using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.U2D.Aseprite;
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
    Vector3 createPoint; // first spawn
    private int clickCount = 0; // 3 ~ 7 char count

    [SerializeField]
    private GameObject CharacterPanel;
    //
    // for destroy
    List<GameObject> setButtons = new List<GameObject>(); 
    private GameObject instantiatedDoneBtn; 
    private GameObject ResetButton; 
    
    private GameObject CreatecountObj;
    private TextMeshProUGUI createCountText;
    
    private GameObject playerObject;
    // for destroy
    

    [SerializeField]
    private Player player;
    void Start()
    {
        playerObject = GameObject.Find("Player");

        createPoint = new Vector3(0, 100, 0);
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
        
        instantiatedDoneBtn = Instantiate(DoneBtn, Vector3.zero, Quaternion.identity, parentTransform); // create end button
        RectTransform doneRect = instantiatedDoneBtn.GetComponent<RectTransform>();
        doneRect.anchorMin = new Vector2(1f, 1f);
        doneRect.anchorMax = new Vector2(1f, 1f);
        doneRect.anchoredPosition = new Vector2(-50f, -25f);

        Button Donebutton = instantiatedDoneBtn.GetComponent<Button>(); // end button click
        if (Donebutton != null)
        {
            Donebutton.onClick.AddListener(onDoneButtonClick);
        }

        ResetButton = Instantiate(ResetBtn, Vector3.zero, Quaternion.identity, parentTransform);
        RectTransform ResetRect = ResetButton.GetComponent<RectTransform>();
        ResetRect.anchorMin = new Vector2(1f, 0f);
        ResetRect.anchorMax = new Vector2(1f, 0f);
        ResetRect.anchoredPosition = new Vector2(-50f, 25f);

        // Reset Button Instantiate
        Button ResetButtonclicked = ResetButton.GetComponent<Button>();
        if (ResetButtonclicked != null)
        {
            ResetButtonclicked.onClick.AddListener(ResetClick);
        }
        
        // text Instantiate
        CreatecountObj = Instantiate(createtextPrefab, Vector3.zero, Quaternion.identity, parentTransform);
        RectTransform countRect = CreatecountObj.GetComponent<RectTransform>();
        countRect.anchorMin = new Vector2(1f, 0.5f);
        countRect.anchorMax = new Vector2(1f, 0.5f);
        countRect.anchoredPosition = new Vector2(-100f, 0f);
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
            DestoryComponent();
        }
        CharacterPanel.SetActive(false);
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
        if(playerObject != null)
        {
            Transform[] children = playerObject.GetComponentsInChildren<Transform>();

            foreach(Transform child in children)
            {
                if (child != playerObject.transform && child.name != "King(Clone)" && !IsDescendantOfKing(child))
                {
                    Destroy(child.gameObject);
                }
            }
        }
        DestoryComponent();
        clickCount = 0;
        Start();
    }
    private bool IsDescendantOfKing(Transform child)
    {
        Transform parent = child.parent;
        while(parent != null)
        {
            if(parent.name == "King(Clone)")
            {
                return true;
            }
            parent = parent.parent;
        }
        return false;
    }

    private void DestoryComponent()
    {
        Destroy(instantiatedDoneBtn);
            foreach(GameObject btn in setButtons)
            {
                Destroy(btn);
            }
        Destroy(ResetButton);
        Destroy(CreatecountObj);
    }
}

