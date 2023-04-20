using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achive { UnlockIrene, UnlockSpectre }
    Achive[] achives; // ���� �����͸� ������ �迭 ����
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); // �����Ұ�
        wait = new WaitForSecondsRealtime(3);
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // ������ �������� �����ϴ� ����Ƽ Ŭ����
        PlayerPrefs.SetInt("MyData", 1); // �������� ������ ������ ���� ����

        foreach (Achive achive in achives) // ���������� �ʱ�ȭ
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
            // achives �迭�� �����͵��� ���������� �ʱ�ȭ����
            // int �� bool�� ��������̹Ƿ� 0�� false�� �ǹ�, �� ���� �ر��� �ȉ�ٴ� ��.
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i=0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            // GetInt �Լ��� ����� ���� ���¸� ������ ��ư Ȱ��ȭ�� ����
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockIrene:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockSpectre:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            // ������ ó�� �޼��Ǿ��� ��
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int i=0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(Noticeroutine());
        }
    }

    IEnumerator Noticeroutine() // �˸�â �����ð����� Ȱ��ȭ �ϴ� �ڷ�ƾ ����
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
