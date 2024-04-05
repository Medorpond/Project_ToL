using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    // 오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹 배열
    public GameObject[] poolObjects;
    // 관리되는 모든 오브젝트를 저장하는 리스트
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
            // 오브젝트가 비활성화되어 있는 경우 활성화
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);

                break;
            }
        }

        // 못 찾았을 경우 생성하고 리스트에 추가
        if (select == null)
        {
            select = Instantiate(poolObjects[index], transform);
            poolItemList[index].Add(select);
        }

        return select;
    }

    // 현재 관리 중인 모든 오브젝트를 삭제
    public void DestroyObjects()
    {
        for (int i = 0; poolItemList.Length < 0; i++)
        {
            // 비어있을 경우
            if (poolItemList[i] == null) continue;

            for (int j = 0; j < poolItemList[i].Count; j++)
            {
                GameObject.Destroy(poolItemList[i][j]);
            }
        }
    }
}
