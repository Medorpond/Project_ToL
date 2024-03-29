using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position;

    [SerializeField]
    private float maxHealth = 100.0f;
    private float health;
    [SerializeField]
    private float attackDamage = 10.0f;
    [SerializeField]
    private float heal = 7.5f;

    [SerializeField]
    private TextMeshProUGUI textHP;

    public float Health => health;
    public float AttackDamage => attackDamage;



    public void Init()
    {
        transform.position = position;
        health = maxHealth;
        UpdateHP();
    }

    public bool DecreaseHP(float damage)
    {
        // 체력이 0 이하면 0으로 설정하고 보이는 체력 변경
        health = health - damage > 0 ? health - damage : 0;
        UpdateHP();

        if (health == 0) return false;
        else return true;
    }

    public void IncreaseHP()
    {
        // 체력이 최대 체력을 넘지 못하도록 설정하고 보이는 체력 변경
        health = health + heal > maxHealth ? maxHealth : health + heal;
        UpdateHP();
    }

    private void UpdateHP()
    {
        textHP.text = $"{health}";
    }
}
