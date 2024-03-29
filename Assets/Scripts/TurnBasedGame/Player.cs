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
        // ü���� 0 ���ϸ� 0���� �����ϰ� ���̴� ü�� ����
        health = health - damage > 0 ? health - damage : 0;
        UpdateHP();

        if (health == 0) return false;
        else return true;
    }

    public void IncreaseHP()
    {
        // ü���� �ִ� ü���� ���� ���ϵ��� �����ϰ� ���̴� ü�� ����
        health = health + heal > maxHealth ? maxHealth : health + heal;
        UpdateHP();
    }

    private void UpdateHP()
    {
        textHP.text = $"{health}";
    }
}
