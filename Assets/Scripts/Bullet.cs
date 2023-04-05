using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public double damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(double damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per; // 관통, -1은 무한

        if (per > -1)
        {
            rigid.velocity = dir * 15f; // 속도 설정
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1) // 충돌한 오브젝트가 몬스터거나, 무기의 관통값이 -1(무한)이 때,
            return;

        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }
}
