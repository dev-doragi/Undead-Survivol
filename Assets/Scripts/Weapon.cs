using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public double damage;
    public int count;
    public float speed;

    float timer;
    Player player; // 플레이어 스크립트 자체를 컴포넌트로 쓸 수 있다.

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed) // 타이머가 다 돌면 총알 발사
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //test
        if (Input.GetButtonDown("Jump"))
            LevelUp(10f, 1);
    }

    public void LevelUp(double damage, int count)
    {
        this.damage = damage;
        this.count += count; // 무기 개수

        if (id == 0)
            Batch();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();

                break;
            default:
                speed = 0.4f; // 발사 속도
                break;
        }
    }

    void Batch() // 회전 무기를 count만큼 배치해주는 함수
    {
        for (int i=0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount) 
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform; // Get()은 부모가 PoolManager로 됨. 부모를 Weapon 0으로 변경
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget) // 가장 가까운 적이 없을 때
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position; // 방향은 적 위치에서 내 위치를 뺀 값
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(4).transform; // 총알 위치 설정
        bullet.position = transform.position; // 플레이어 위치와 동일
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
