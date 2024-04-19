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
    private GameObject[] CharBtn;
    [SerializeField]
    private GameObject DoneBtn;
    Vector3 createPoint = new Vector3(0, 100, 0);
    private int x = 0;
    private int clickCount = 0;
    List<GameObject> setButtons = new List<GameObject>();
    List<GameObject> createdBtns = new List<GameObject>();
    GameObject instantiatedDoneBtn;
    void Start()
    {
        Transform parentTransform = GameObject.Find("SelectChar").transform;
        for(int i = 0; i < CharBtn.Length; i++)
        {
            GameObject instantiatedBtn = Instantiate(CharBtn[i], Vector3.zero, Quaternion.identity, parentTransform);
            instantiatedBtn.transform.localPosition = createPoint; // local location set
            createPoint.x += 100;
            setButtons.Add(instantiatedBtn);

            Button button = instantiatedBtn.GetComponent<Button>();
            if (button != null)
            {
                int index = i;
                button.onClick.AddListener(() => OnButtonClick(index));
            }
        }
        createPoint = new Vector3(710, 175, 0);
        instantiatedDoneBtn = Instantiate(DoneBtn, Vector3.zero, Quaternion.identity, parentTransform);
        instantiatedDoneBtn.transform.localPosition = createPoint;

        Button Donebutton = instantiatedDoneBtn.GetComponent<Button>();
        if (Donebutton != null)
        {
            Donebutton.onClick.AddListener(onDoneButtonClick);
        }
    }

    void OnButtonClick(int index){
        if(index == 0){
            Transform parent = GameObject.Find("SelectChar").transform;
            GameObject TankerButton = Instantiate(CharBtn[index], Vector3.zero, Quaternion.identity, parent);
            TankerButton.transform.localPosition = new Vector3(x, 0, 0);
            x += 100;
            createdBtns.Add(TankerButton);
        }
        else if (index == 1){
            Transform parent = GameObject.Find("SelectChar").transform;
            GameObject DealerButton = Instantiate(CharBtn[index], Vector3.zero, Quaternion.identity, parent);
            DealerButton.transform.localPosition = new Vector3(x, 0, 0);
            x += 100;
            createdBtns.Add(DealerButton);
        }
        else if (index == 2){
            Transform parent = GameObject.Find("SelectChar").transform;
            GameObject HealerButton = Instantiate(CharBtn[index], Vector3.zero, Quaternion.identity, parent);
            HealerButton.transform.localPosition = new Vector3(x, 0, 0);
            x += 100;
            createdBtns.Add(HealerButton);
        }
        clickCount++;
        if (clickCount >= 7)
        {
            onDoneButtonClick();
        }
    }
    void onDoneButtonClick(){
        Destroy(instantiatedDoneBtn);
        foreach(GameObject btn in setButtons)
        {
            Destroy(btn);
        }
        foreach(GameObject btn in createdBtns)
        {
            btn.transform.localPosition += new Vector3(20f, 50f, 0f);
        }
    }
}
