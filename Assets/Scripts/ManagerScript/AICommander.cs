using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommander : MonoBehaviour
{
    List<GameObject> psudoList = new List<GameObject>();
    List<GameObject> psudoEnemyList = new List<GameObject>();

    public string Analize()
    {
        int maxWeight = 0;
        string command = "Idle";
        foreach (GameObject unit in psudoList)
        {
            string result = AnalizeUnit(unit, ref maxWeight);
            if (result != null)
            {
                command = result;
            }
        }
        return command;


        
    }

    string AnalizeUnit(GameObject unit, ref int maxWeight)
    {
        // StartPoint
        int actionWeight = 0;
        Unit unitScript = unit.GetComponent<Unit>();
        foreach (Node node in unitScript.movableNode)
        {
            foreach (GameObject enemy in psudoEnemyList)
            {
                int currentWeight = 0;
                float distance =  Vector2.Distance(unit.transform.position, enemy.transform.position);
                if (unitScript.attackRange <= distance) WeighAttack(enemy, ref currentWeight);
                if (unitScript.ability2Range <= distance) WeighAbility(enemy, ref currentWeight);
                actionWeight = Mathf.Max(actionWeight, currentWeight);
            }
        }
        //Command Update should be made.
        //Do I really need that much Weight variables? Couldn't this just be Mathf.Max(maxWeight, currentWeight)?



        // EndPoint
        if (actionWeight > maxWeight)
        {
            maxWeight = actionWeight;
            return "";
        }
        else
        {
            return null;
        }
    }

    void WeighAttack(GameObject enemy, ref int currentWeight)
    {
        int attackWeight = 0;
        //Analize Attack Method's Weight
        currentWeight = Mathf.Max(currentWeight, attackWeight);
    }

    void WeighAbility(GameObject enemy, ref int currentWeight)
    {
        int abilityWeight = 0;
        //Analize Ability2 (Battle Ability)'s Weight
        currentWeight = Mathf.Max(currentWeight, abilityWeight);
    }
}
