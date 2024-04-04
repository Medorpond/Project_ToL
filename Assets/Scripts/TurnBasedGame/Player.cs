using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position;

    // �ִ� ü��, ü��, ������, ���� ��
    [SerializeField]
    private int maxHealth = 100;
    private int health;
    [SerializeField]
    private int attackDamage = 10;
    [SerializeField]
    private int heal = 7;

    // ü��, ������ ǥ�� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private GameObject textDamage;

    public int Health => health;
    public int AttackDamage => attackDamage;



    public void Init()
    {
        transform.position = position;
        health = maxHealth;
        UpdateHP();
    }

    public bool DecreaseHP(int damage)
    {
        // ���� ������ ���� (�÷��̾�� ����)
        GameObject damageHUD = Instantiate(textDamage, transform);
        damageHUD.GetComponent<DamageText>().damage = damage;
        damageHUD.transform.position = gameObject.transform.position + Vector3.up * 1.0f;

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
