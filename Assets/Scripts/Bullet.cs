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
        this.per = per; // ����, -1�� ����

        if (per > -1)
        {
            rigid.velocity = dir * 15f; // �ӵ� ����
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1) // �浹�� ������Ʈ�� ���Ͱų�, ������ ���밪�� -1(����)�� ��,
            return;

        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }
}
