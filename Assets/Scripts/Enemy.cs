using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public double health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;
    void Awake() // 초기화
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable() // 몬스터 활성화, 생존과 체력 초기화됨
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // 타겟 지정, 게임매니저에서 따옴
        isLive = true;
        health = maxHealth;
    }

    // spawner에서 초기 속성(Spawn Data)을 적용하는 함수
    public void Init(SpawnData data) // SpawnData의 정보를 data변수에 저장
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // 몬스터의 종류(스프라이트)  ex) 0은 좀비, 1은 해골
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) // Bullet과 충돌하지 않았을 때
            return;

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {

        }
        else
        {
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
