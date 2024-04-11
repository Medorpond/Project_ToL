using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeHPText : MonoBehaviour
{
    private TextMeshPro changeText;

    // 데미지 텍스트 효과를 위해 트윈 그래프 함수 적용 예정
    [SerializeField]
    private AnimationCurve fadeCurve;
    private float fadeTime = 5.0f;

    public int changeHP;



    void Start()
    {
        changeText = GetComponent<TextMeshPro>();
        Setup();

        // 일정 시간 지난 후 사라짐
        Invoke("DestroyObject", fadeTime + 1.0f);
    }

    private void Update()
    {
        StartCoroutine("Fade");
    }

    private void Setup()
    {
        if (changeHP > 0)   // heal
        {
            changeText.color = Color.blue;
            changeText.text = $"{changeHP}";
        }
        else   // deal
        {
            changeText.color = Color.red;
            changeText.text = $"{-changeHP}";
        }
    }

    // 서서히 글자가 사라지게 함
    private IEnumerator Fade()
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = changeText.color;
            color.a = Mathf.Lerp(1, 0, fadeCurve.Evaluate(percent));
            changeText.color = color;

            yield return null;
        }
    }

    // 일정 시간 후에 오브젝트 파괴
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
