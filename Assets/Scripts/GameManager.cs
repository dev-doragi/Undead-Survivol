using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 전역으로 GameManager클래스형의 instance 변수 선언
    [Header("# Game Control")]
    public bool isLive; // 시간 정지 여부를 알려주는 bool 값
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp UILevelUP;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;

        // 임시 스크립트
        UILevelUP.Select(0); // 기본 무기 삽 제공
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        // 레벨 업 로직, next exp는 최대 10개가 끝이기 때문에, 10레벨 부터는 레벨업을 해도 10레벨
        {
            level++;
            exp = 0;
            UILevelUP.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // 유니티 내 시간 정지

    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // 유니티 내 시간 지속
    }
}
