using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        // 배열 속 각 리스트들을 초기화하는 반복문
        for (int i = 0; i < pools.Length; i++) 
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int i)
    {
        GameObject select = null;

        // 선택한 풀의 비활성화된 게임오브젝트 접근
        foreach (GameObject item in pools[i])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 발견 못할시 
        if (!select) // select == null 일 때 작동
        {
            // 새롭게 생성 후 select 변수에 할당
            select = Instantiate(prefabs[i], transform);
            pools[i].Add(select);
        }

        return select;
    }

}
