using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true); // 비활성화된 애들도 가져와야 하니 true 인자값 넣기.
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int i)
    {
        items[i].OnClick();
    }

    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        // 2. 랜덤 아이템 3개 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            if (ran[0]!=ran[1] && ran[1]!=ran[2] && ran[0]!=ran[2]) // 3가지 아이템이 겹치지 않을 때
                break;
        }

        for (int i=0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            // 3. 만렙이 될 경우 소비 아이템(힐)으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
                //items[Random.RandomRange(4, 7)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }

    }
}
