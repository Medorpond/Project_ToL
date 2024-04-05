using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������ �迭
    public GameObject[] poolObjects;
    // �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ
    private List<GameObject>[] poolItemList;



    private void Awake()
    {
        poolItemList = new List<GameObject>[poolObjects.Length];

        for (int i = 0;  i < poolItemList.Length; i++)
        {
            poolItemList[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject item in poolItemList[index])
        {
            // ������Ʈ�� ��Ȱ��ȭ�Ǿ� �ִ� ��� Ȱ��ȭ
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);

                break;
            }
        }

        // �� ã���� ��� �����ϰ� ����Ʈ�� �߰�
        if (select == null)
        {
            select = Instantiate(poolObjects[index], transform);
            poolItemList[index].Add(select);
        }

        return select;
    }

    // ���� ���� ���� ��� ������Ʈ�� ����
    public void DestroyObjects()
    {
        for (int i = 0; poolItemList.Length < 0; i++)
        {
            // ������� ���
            if (poolItemList[i] == null) continue;

            for (int j = 0; j < poolItemList[i].Count; j++)
            {
                GameObject.Destroy(poolItemList[i][j]);
            }
        }
    }
}
