using System.Collections;
using System.Collections.Generic;
using Amazon.GameLift.Model;
using TMPro;
using UnityEngine;

public class ResultUI_Count : MonoBehaviour
{
    public PlayerManager playerMg;
    public OpponentManager opponentMg;

    private int Player_Knight_Count;
    private int Player_Archer_Count;
    private int Player_AxeGiant_Count;
    private int Player_Priest_Count;
    private int Player_Assassin_Count;
    private int Player_Magician_Count;
    private int Player_Shield_Count;
    private int Enemy_Knight_Count;
    private int Enemy_Archer_Count;
    private int Enemy_AxeGiant_Count;
    private int Enemy_Priest_Count;
    private int Enemy_Assassin_Count;
    private int Enemy_Magician_Count;
    private int Enemy_Shield_Count;

    private int Player_Total;
    private int Enemy_Total;

    [SerializeField]
    private TMP_Text Player_Knight_text;
    [SerializeField]
    private TMP_Text Player_Archer_text;
    [SerializeField]
    private TMP_Text Player_AxeGiant_text;
    [SerializeField]
    private TMP_Text Player_Priest_text;
    [SerializeField]
    private TMP_Text Player_Assassin_text;
    [SerializeField]
    private TMP_Text Player_Magician_text;
    [SerializeField]
    private TMP_Text Player_Shield_text;

    [SerializeField]
    private TMP_Text Enemy_Knight_text;
    [SerializeField]
    private TMP_Text Enemy_Archer_text;
    [SerializeField]
    private TMP_Text Enemy_AxeGiant_text;
    [SerializeField]
    private TMP_Text Enemy_Priest_text;
    [SerializeField]
    private TMP_Text Enemy_Assassin_text;
    [SerializeField]
    private TMP_Text Enemy_Magician_text;
    [SerializeField]
    private TMP_Text Enemy_Shield_text;

    [SerializeField]
    private TMP_Text Player_Total_text;
    [SerializeField]
    private TMP_Text Enemy_Total_text;

    private void Awake() 
    {
        if (playerMg == null || opponentMg == null)
        {
            Debug.LogError("PlayerManager or opponentManager not allocate");
            return;
        }

        CountUnits();

        Player_Knight_text.text = Player_Knight_Count.ToString();
        Player_Archer_text.text = Player_Archer_Count.ToString();
        Player_AxeGiant_text.text = Player_AxeGiant_Count.ToString();
        Player_Priest_text.text = Player_Priest_Count.ToString();
        Player_Assassin_text.text = Player_Assassin_Count.ToString();
        Player_Magician_text.text = Player_Magician_Count.ToString();
        Player_Shield_text.text = Player_Shield_Count.ToString();

        Enemy_Knight_text.text = Enemy_Knight_Count.ToString();
        Enemy_Archer_text.text = Enemy_Archer_Count.ToString();
        Enemy_AxeGiant_text.text = Enemy_AxeGiant_Count.ToString();
        Enemy_Priest_text.text = Enemy_Priest_Count.ToString();
        Enemy_Assassin_text.text = Enemy_Assassin_Count.ToString();
        Enemy_Magician_text.text = Enemy_Magician_Count.ToString();
        Enemy_Shield_text.text = Enemy_Shield_Count.ToString();

        Player_Total_text.text = Player_Total.ToString();
        Enemy_Total_text.text = Enemy_Total.ToString();
    }
    
    private void CountUnits()
    {
        Player_Knight_Count = CountUnitsByName(playerMg.transform, "Knight");
        Player_Archer_Count = CountUnitsByName(playerMg.transform, "Archer");
        Player_AxeGiant_Count = CountUnitsByName(playerMg.transform, "AxeGiant");
        Player_Priest_Count = CountUnitsByName(playerMg.transform, "Priest");
        Player_Assassin_Count = CountUnitsByName(playerMg.transform, "Assassin");
        Player_Magician_Count = CountUnitsByName(playerMg.transform, "Magician");
        Player_Shield_Count = CountUnitsByName(playerMg.transform, "Shield");
        
        Enemy_Knight_Count = CountUnitsByName(opponentMg.transform, "Knight");
        Enemy_Archer_Count = CountUnitsByName(opponentMg.transform, "Archer");
        Enemy_AxeGiant_Count = CountUnitsByName(opponentMg.transform, "AxeGiant");
        Enemy_Priest_Count = CountUnitsByName(opponentMg.transform, "Priest");
        Enemy_Assassin_Count = CountUnitsByName(opponentMg.transform, "Assassin");
        Enemy_Magician_Count = CountUnitsByName(opponentMg.transform, "Magician");
        Enemy_Shield_Count = CountUnitsByName(opponentMg.transform, "Shield");

        Player_Total = Player_Knight_Count + Player_Archer_Count + Player_AxeGiant_Count + 
            Player_Priest_Count + Player_Assassin_Count + Player_Magician_Count + Player_Shield_Count;
        
        Enemy_Total = Enemy_Knight_Count + Enemy_Archer_Count + Enemy_AxeGiant_Count + 
            Enemy_Priest_Count + Enemy_Assassin_Count + Enemy_Magician_Count + Enemy_Shield_Count;
    }

    private int CountUnitsByName(Transform parent, string unitName)
    {
        int count = 0;
        foreach (Transform child in parent)
        {
            if (child.name.Contains(unitName))
            {
                count++;
            }
        }
        return count;
    }
}
