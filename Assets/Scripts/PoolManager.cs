using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ������ ������ ����
    public GameObject[] prefabs;

    // Ǯ ����� �ϴ� ����Ʈ
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        // �迭 �� �� ����Ʈ���� �ʱ�ȭ�ϴ� �ݺ���
        for (int i = 0; i < pools.Length; i++) 
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int i)
    {
        GameObject select = null;

        // ������ Ǯ�� ��Ȱ��ȭ�� ���ӿ�����Ʈ ����
        foreach (GameObject item in pools[i])
        {
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // �߰� ���ҽ� 
        if (!select) // select == null �� �� �۵�
        {
            // ���Ӱ� ���� �� select ������ �Ҵ�
            select = Instantiate(prefabs[i], transform);
            pools[i].Add(select);
        }

        return select;
    }

}
