using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject[] CharBtn; // prefabs
    [SerializeField]
    private GameObject DoneBtn; // done button
    Vector3 createPoint = new Vector3(0, 100, 0); // first spawn
    private int x = 0; // + 100
    private int clickCount = 0; // 3 ~ 7 char count
    List<GameObject> setButtons = new List<GameObject>(); // for destroy
    List<GameObject> createdBtns = new List<GameObject>(); // for reference
    GameObject instantiatedDoneBtn; // for destroy
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

    }

    void OnButtonClick(int index){ // click 1-level button(choose)
            if(index == 0){ // ex) 1 = tanker, 2 = dealer, ...
            Transform parent = GameObject.Find("SelectChar").transform;       // sub SelectChar
            GameObject TankerButton = Instantiate(CharBtn[index], Vector3.zero, Quaternion.identity, parent); // create 2-level button (tanker)
            TankerButton.transform.localPosition = new Vector3(x, 0, 0);    // left-bottom
            x += 100;   // next + 100
            createdBtns.Add(TankerButton); // List add 
            Button buttonComponent = TankerButton.GetComponent<Button>(); // click 2-level button (select)
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() =>
                {
                    Debug.Log("TankerButton Clicked!"); // print log
                });
            }
            }
        else if (index == 1){ // ex) 1 = tanker, 2 = dealer, ...
            Transform parent = GameObject.Find("SelectChar").transform;
            GameObject DealerButton = Instantiate(CharBtn[index], Vector3.zero, Quaternion.identity, parent);
            DealerButton.transform.localPosition = new Vector3(x, 0, 0);
            x += 100;
            createdBtns.Add(DealerButton);
            Button buttonComponent = DealerButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() =>
                {
                    Debug.Log("DealerButton Clicked!");
                });
            }
            }
        else if (index == 2){
            Transform parent = GameObject.Find("SelectChar").transform;
            GameObject HealerButton = Instantiate(CharBtn[index], Vector3.zero, Quaternion.identity, parent);
            HealerButton.transform.localPosition = new Vector3(x, 0, 0);
            x += 100;
            createdBtns.Add(HealerButton);
            Button buttonComponent = HealerButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() =>
                {
                    Debug.Log("HealerButton Clicked!");
                });
            }
        }
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
            foreach(GameObject btn in createdBtns)
            {
                btn.transform.localPosition += new Vector3(20f, 50f, 0f); // move 2-level button
            }
        }
        
    }
}
