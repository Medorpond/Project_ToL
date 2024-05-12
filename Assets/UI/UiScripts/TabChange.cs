using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TabChange : MonoBehaviour
{
    [SerializeField] private Selectable[] selectables;
    private bool isLoginCalled = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable currnetSelectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            if (currnetSelectable != null)
            {
                int currentIndex = System.Array.IndexOf(selectables, currnetSelectable);
                if (currentIndex != -1)
                {
                    int nextIndex = (currentIndex + 1) % selectables.Length;
                    selectables[nextIndex].Select();
                }
            }
        }

        if (!isLoginCalled && Input.GetKeyDown(KeyCode.Return))
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
            if (currentSelected != null)
            {
                Button clickedButton = currentSelected.GetComponent<Button>();
                if (clickedButton != null)
                {
                    clickedButton.onClick.Invoke();
                    isLoginCalled = true;
                    StartCoroutine(ResetLoginCalled());
                }
            }
        }
    }
    IEnumerator ResetLoginCalled()
    {
        yield return new WaitForSeconds(0.1f);
        isLoginCalled = false;
    }
}
