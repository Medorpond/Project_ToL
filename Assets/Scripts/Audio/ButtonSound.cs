using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSoun : MonoBehaviour
{
    

public class ButtonSoundManager : MonoBehaviour
{
    void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => AudioManager.instance.PlaySfx(AudioManager.Sfx.sfx_click_ui));

            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { AudioManager.instance.PlaySfx(AudioManager.Sfx.sfx_mouse_on_ui); });
            trigger.triggers.Add(entry);
        }
    }

    
}

}
