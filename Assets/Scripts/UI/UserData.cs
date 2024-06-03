using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/UserData", order = 1)]
public class  UserData : ScriptableObject
{
    public string username;
    public string SUB;
    public string WIN;
    public string LOSE;
}
