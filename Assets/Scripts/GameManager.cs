using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �������� GameManagerŬ�������� instance ���� ����
    [Header("# Game Control")]
    public bool isLive; // �ð� ���� ���θ� �˷��ִ� bool ��
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

        // �ӽ� ��ũ��Ʈ
        UILevelUP.Select(0); // �⺻ ���� �� ����
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
        // ���� �� ����, next exp�� �ִ� 10���� ���̱� ������, 10���� ���ʹ� �������� �ص� 10����
        {
            level++;
            exp = 0;
            UILevelUP.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // ����Ƽ �� �ð� ����

    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // ����Ƽ �� �ð� ����
    }
}
