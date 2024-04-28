using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Character character;
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
        kingHP = character.Health;
    }

    // temp for Move
    private void Update()
    {
        transform.position = character.location;
        kingHP = character.Health;
    }

    public void SetCharacter(GameObject characterClass)
    {
        Instantiate(characterClass, transform);
        character = GetComponentInChildren<Character>();

        // username (change)
        userName.text = $"{character.GetType().Name}";

        transform.position = character.location;
        character.Init();
        textHP.text = $"{character.Health}";
        // yong
        textClass.text = $"{character.GetType().Name}";
        textATK.text = $"{character.AttackDamage}";
        textmovement.text = $"{character.MoveRange}";
        // ~yong
        character.onHPEvent.AddListener(UpdateHP);
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
