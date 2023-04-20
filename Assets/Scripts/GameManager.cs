using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 전역으로 GameManager클래스형의 instance 변수 선언
    [Header("# Game Control")]
    public bool isLive; // 시간 정지 여부를 알려주는 bool 값
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
        isLive = false; // 사망 처리

        yield return new WaitForSeconds(0.5f); // 게임 멈추기 전에 잠깐 대기하는 루틴

        uiResult.gameObject.SetActive(true); // 게임오버 ui 호출
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
        isLive = false; // 사망 처리
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f); // 게임 멈추기 전에 잠깐 대기하는 루틴

        uiResult.gameObject.SetActive(true); // 게임오버 ui 호출
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
        // 씬의 인덱스를 파라미터로 사용 가능
        // 임시 스크립트 (첫번째 캐릭터)
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
