using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner; // Scanner��� ��ũ��Ʈ�� ������Ʈ�� ���
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void FixedUpdate() // ���� ���� �����Ӹ��� ȣ��Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager.instance.isLive)
            return;
        //// 1. ��
        //rigid.AddForce(inputVec);

        //// 2. �ӵ� ����
        //rigid.velocity = inputVec;

        // 3, ��ġ ����
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec);
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    //void OnMove(InputValue value) inputVec.magnitude �� ������ �ȵȴ�. �� ������ �̵��� �˾ƺ���.
    //{
    //    inputVec = value.Get<Vector2>();
    //}

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
