using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public List<GameObject> characters;
    public Character kingCharacter;

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


    // temp for king unit
    [SerializeField]
    private GameObject kingUnit;
    [SerializeField]
    private Vector2 kingSpawnPoint;



    // at first add king unit
    private void Awake()
    {
        GameObject king = Instantiate(kingUnit, transform);
        characters.Add(king);

        kingCharacter = characters[0].GetComponentInChildren<Character>();
        kingCharacter.Init();
        king.transform.position = kingSpawnPoint;
        kingCharacter.onHPEvent.AddListener(UpdateHP);

        UpdateUI();
    }

    public void SetCharacter(GameObject characterClass)
    {
        GameObject newCharacter = Instantiate(characterClass, transform);
        characters.Add(newCharacter);
        newCharacter.GetComponentInChildren<Character>().Init();

        // yong
        // UpdateUI();
        // ~yong
    }

    private void UpdateUI() // update UI component
    {
        textHP.text = $"{kingCharacter.Health}";
        textClass.text = $"{kingCharacter.GetType().Name}";
        textATK.text = $"{kingCharacter.AttackDamage}";
        textmovement.text = $"{kingCharacter.MoveRange}";
    }

    public bool TakeDamage(int damage)
    {
        //return characters[index].GetComponentInChildren<Character>().DecreaseHP(damage);

        return kingCharacter.DecreaseHP(damage);
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

    public bool IsKingLive()
    {
        if (kingCharacter.Health > 0) return true;
        else return false;
    }

    private void OnMouseDown()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            GameObject click_obj = hit.transform.gameObject;
            Debug.Log(click_obj.name);
        }
    }

    public int GetIndex(GameObject selectedUnit)
    {   
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == selectedUnit) return i;
        }

        return -1;
    }
}
