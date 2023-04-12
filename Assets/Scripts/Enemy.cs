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

    void Awake() // �ʱ�ȭ
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

    void OnEnable() // ���� Ȱ��ȭ, ������ ü�� �ʱ�ȭ��
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // Ÿ�� ����, ���ӸŴ������� ����
        isLive = true;
        health = maxHealth;

        isLive = true;
        coll.enabled = true; // ������Ʈ�� ��Ȱ��ȭ�� .enaled = false;
        rigid.simulated = true; // ������ٵ��� ������ ��Ȱ��ȭ�� .simulated = false;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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
        if (!collision.CompareTag("Bullet") || !isLive) // Bullet�� �浹���� �ʰų�, �׾����� ���� �ɷ���
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());
        // StartCoroutine("KnockBack");

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else // �׾��� ��
        {
            isLive = false;
            coll.enabled = false; // ������Ʈ�� ��Ȱ��ȭ�� .enaled = false;
            rigid.simulated = false; // ������ٵ��� ������ ��Ȱ��ȭ�� .simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
