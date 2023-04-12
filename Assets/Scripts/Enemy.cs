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
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake() // 초기화
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive || !isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable() // 몬스터 활성화, 생존과 체력 초기화됨
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // 타겟 지정, 게임매니저에서 따옴
        isLive = true;
        health = maxHealth;

        isLive = true;
        coll.enabled = true; // 컴포넌트의 비활성화는 .enaled = false;
        rigid.simulated = true; // 리지드바디의 물리적 비활성화는 .simulated = false;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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
        if (!collision.CompareTag("Bullet") || !isLive) // Bullet과 충돌하지 않거나, 죽어있을 때는 걸러짐
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());
        // StartCoroutine("KnockBack");

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else // 죽었을 때
        {
            isLive = false;
            coll.enabled = false; // 컴포넌트의 비활성화는 .enaled = false;
            rigid.simulated = false; // 리지드바디의 물리적 비활성화는 .simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
