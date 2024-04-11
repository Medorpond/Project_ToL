using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject characterClass;
    [HideInInspector]
    public Character character;

    [SerializeField]
    private TextMeshProUGUI userName;

    // health, damaget Text
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private GameObject changeHPtext;



    private void Awake()
    {
        Instantiate(characterClass, transform);
        character = GetComponentInChildren<Character>();

        // username (change)
        userName.text = $"{character.GetType().Name}";

        transform.position = character.location;
        character.Init();
        textHP.text = $"{character.Health}";

        character.onHPEvent.AddListener(UpdateHP);
    }

    // temp for Move
    private void Update()
    {
        transform.position = character.location;
    }

    public bool TakeDamage(int damage)
    {
        return character.DecreaseHP(damage);
    }

    private void UpdateHP(int previousHP, int currentHP)
    {
        if (previousHP != currentHP)
        {
            // make UI
            GameObject healthHUD = Instantiate(changeHPtext, transform);
            healthHUD.GetComponent<ChangeHPText>().changeHP = currentHP - previousHP;
            healthHUD.transform.position = gameObject.transform.position + Vector3.up * 1.0f;
        }

        textHP.text = $"{currentHP}";
    }
}
