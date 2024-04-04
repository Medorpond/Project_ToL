using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position;

    // 최대 체력, 체력, 데미지, 힐량 등
    [SerializeField]
    private int maxHealth = 100;
    private int health;
    [SerializeField]
    private int attackDamage = 10;
    [SerializeField]
    private int heal = 7;

    // 체력, 데미지 표기 텍스트
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
        // 받은 데미지 생성 (플레이어보다 위에)
        GameObject damageHUD = Instantiate(textDamage, transform);
        damageHUD.GetComponent<DamageText>().damage = damage;
        damageHUD.transform.position = gameObject.transform.position + Vector3.up * 1.0f;

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
