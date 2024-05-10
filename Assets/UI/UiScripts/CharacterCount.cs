using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCount : MonoBehaviour
{
    [SerializeField]
    private Transform MyCharParents;
    [SerializeField]
    private Transform EnemyCharParents;
    [SerializeField]
    private TextMeshProUGUI MycharacterCountText;
    [SerializeField]
    private TextMeshProUGUI EnemycharacterCountText;

    void Update()
    {
        int MycharacterCount = MyCharParents.childCount;
        int EnemycharacterCount = EnemyCharParents.childCount;

        MycharacterCountText.text = "Characters : " + MycharacterCount.ToString();
        EnemycharacterCountText.text = EnemycharacterCount.ToString() + " : Characters";   
    }
}
