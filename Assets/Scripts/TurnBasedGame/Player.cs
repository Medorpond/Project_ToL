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

    // health, damage Text
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
    private Vector3 kingSpawnPoint;



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
        newCharacter.name = $"{characterClass.name}{characters.Count}";
        characters.Add(newCharacter);

        // set newCharacter
        newCharacter.GetComponent<Character>().location += new Vector2(characters.Count - 1, 0);
        newCharacter.GetComponentInChildren<Character>().Init();
    }

    private void UpdateUI() // update UI component
    {
        textHP.text = $"{kingCharacter.Health}";
        textClass.text = $"{kingCharacter.GetType().Name}";
        textATK.text = $"{kingCharacter.AttackDamage}";
        textmovement.text = $"{kingCharacter.MoveRange}";
    }

    private void UpdateHP(int previousHP, int currentHP)
    {
        if (previousHP != currentHP)
        {
            // make UI
            GameObject healthHUD = Instantiate(changeHPtext, transform);
            healthHUD.GetComponent<ChangeHPText>().changeHP = currentHP - previousHP;
            // make above the king
            healthHUD.transform.position = kingCharacter.location + new Vector2(0, 1);
        }

        textHP.text = $"{currentHP}";
    }

    public bool IsKingDead()
    {
        if (kingCharacter.Health > 0) return false;
        else return true;
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
