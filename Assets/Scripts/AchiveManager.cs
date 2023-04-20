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
    Achive[] achives; // 업적 데이터를 저장할 배열 선언
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); // 복습할것
        wait = new WaitForSecondsRealtime(3);
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // 간단한 저장기능을 제공하는 유니티 클래스
        PlayerPrefs.SetInt("MyData", 1); // 도전과제 데이터 유지를 위한 변수

        foreach (Achive achive in achives) // 순차적으로 초기화
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
            // achives 배열의 데이터들을 순차적으로 초기화해줌
            // int 와 bool은 상관관계이므로 0은 false를 의미, 즉 아직 해금이 안됬다는 뜻.
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
            // GetInt 함수로 저장된 업적 상태를 가져와 버튼 활성화에 적용
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
            // 업적이 처음 달성되었을 때
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int i=0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(Noticeroutine());
        }
    }

    IEnumerator Noticeroutine() // 알림창 일정시간동안 활성화 하는 코루틴 생성
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
