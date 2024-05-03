using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Character character;
    public List<GameObject> characters;
    [HideInInspector]
    public int kingHP;

    [SerializeField]
    private TextMeshProUGUI userName;

    // health, damaget Text
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private GameObject changeHPtext;
    
    // yong ~ 
    [SerializeField]
    private TextMeshProUGUI textClass;
    [SerializeField]
    private TextMeshProUGUI textATK;
    [SerializeField]
    private TextMeshProUGUI textmovement;
    // ~ yong



    private void Awake()
    {
        //SetCharacter();
        kingHP = 10;
    }

    public void SetCharacter(GameObject characterClass)
    {
        Instantiate(characterClass, transform);
        character = GetComponentInChildren<Character>();

        // username (change)
        transform.position = character.location;
        character.Init();
        // yong
        UpdateUI();
        // ~yong
        character.onHPEvent.AddListener(UpdateHP);
    }

    private void UpdateUI() // update UI component
    {
        textHP.text = $"{character.Health}";
        textClass.text = $"{character.GetType().Name}";
        textATK.text = $"{character.AttackDamage}";
        textmovement.text = $"{character.MoveRange}";
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
