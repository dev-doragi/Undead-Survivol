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
    void Awake() // �ʱ�ȭ
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

    void OnEnable() // ���� Ȱ��ȭ, ������ ü�� �ʱ�ȭ��
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // Ÿ�� ����, ���ӸŴ������� ����
        isLive = true;
        health = maxHealth;
    }

    // spawner���� �ʱ� �Ӽ�(Spawn Data)�� �����ϴ� �Լ�
    public void Init(SpawnData data) // SpawnData�� ������ data������ ����
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // ������ ����(��������Ʈ)  ex) 0�� ����, 1�� �ذ�
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) // Bullet�� �浹���� �ʾ��� ��
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
