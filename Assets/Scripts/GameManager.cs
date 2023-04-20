using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �������� GameManagerŬ�������� instance ���� ����
    [Header("# Game Control")]
    public bool isLive; // �ð� ���� ���θ� �˷��ִ� bool ��
    public float gameTime;
    public float maxGameTime = 6 * 10f;
    [Header("# Player Info")]
    public int playerID;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp UILevelUP;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
    }

    public void GameStart(int id)
    {
        playerID = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        switch (playerID)
        {
            case 4:
                UILevelUP.Select(7);
                break;
            case 5:
                UILevelUP.Select(9);
                break;
            default:
                UILevelUP.Select(playerID % 2);
                break;

        }
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false; // ��� ó��

        yield return new WaitForSeconds(0.5f); // ���� ���߱� ���� ��� ����ϴ� ��ƾ

        uiResult.gameObject.SetActive(true); // ���ӿ��� ui ȣ��
        uiResult.Lose();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        yield return new WaitForSeconds(5.5f);
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false; // ��� ó��
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f); // ���� ���߱� ���� ��� ����ϴ� ��ƾ

        uiResult.gameObject.SetActive(true); // ���ӿ��� ui ȣ��
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
        // ���� �ε����� �Ķ���ͷ� ��� ����
        // �ӽ� ��ũ��Ʈ (ù��° ĳ����)
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

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
