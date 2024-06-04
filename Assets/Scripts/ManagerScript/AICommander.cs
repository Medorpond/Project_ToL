using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommander : MonoBehaviour
{
    List<GameObject> psudeList = new List<GameObject>();

    public string Analize()
    {
        int maxWeight = 0;
        string command = "Idle";
        foreach (GameObject unit in psudeList)
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
        List<Node> movableNode = new();




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
}
