using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private TextMeshPro textDamage;

    // ������ �ؽ�Ʈ ȿ���� ���� Ʈ�� �׷��� �Լ� ���� ����
    [SerializeField]
    private AnimationCurve fadeCurve;
    private float fadeTime = 5.0f;

    public int damage;



    void Start()
    {
        textDamage = GetComponent<TextMeshPro>();
        textDamage.text = damage.ToString();

        // ���� �ð� ���� �� �����
        // Pool�� ���� ����
        Invoke("DestroyObject", fadeTime + 1.0f);
    }

    private void Update()
    {
        StartCoroutine("Fade");
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

            Color color = textDamage.color;
            color.a = Mathf.Lerp(1, 0, fadeCurve.Evaluate(percent));
            textDamage.color = color;

            yield return null;
        }
    }

    // ���� �ð� �Ŀ� ������Ʈ �ı�
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
