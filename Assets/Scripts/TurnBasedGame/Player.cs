using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Character character;

    [SerializeField]
    private TextMeshProUGUI userName;

    // health, damaget Text
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private GameObject textDamage;



    private void Awake()
    {
        character = GetComponent<Character>();

        // username (change)
        userName.text = $"{character.GetType().Name}";

        transform.position = character.position;
        character.Init();

        character.onHPEvent.AddListener(UpdateHP);
    }

    public bool TakeDamage(int damage)
    {
        // make damage UI
        GameObject damageHUD = Instantiate(textDamage, transform);
        damageHUD.GetComponent<DamageText>().damage = damage;
        damageHUD.transform.position = gameObject.transform.position + Vector3.up * 1.0f;

        bool isLive = character.DecreaseHP(damage);
        return isLive;
    }

    private void UpdateHP(int health)
    {
        textHP.text = $"{health}";
    }
}
