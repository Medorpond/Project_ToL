using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weatherCondition
{
    sunny, cloudy, rainy, foggy
}

public class Weather : MonoBehaviour
{
    public weatherCondition weather;

    private void Awake()
    {
        // weather �������� ����
        weather = (weatherCondition)Random.Range(0, 4);
        Debug.Log($"Weather is {weather}");
    }
}
