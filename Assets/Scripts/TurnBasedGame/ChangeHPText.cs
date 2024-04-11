using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeHPText : MonoBehaviour
{
    private TextMeshPro changeText;

    // ������ �ؽ�Ʈ ȿ���� ���� Ʈ�� �׷��� �Լ� ���� ����
    [SerializeField]
    private AnimationCurve fadeCurve;
    private float fadeTime = 5.0f;

    public int changeHP;



    void Start()
    {
        changeText = GetComponent<TextMeshPro>();
        Setup();

        // ���� �ð� ���� �� �����
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

    // ������ ���ڰ� ������� ��
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

    // ���� �ð� �Ŀ� ������Ʈ �ı�
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
