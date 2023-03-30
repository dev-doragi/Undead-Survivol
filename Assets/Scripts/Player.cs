using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() // 물리 연산 프레임마다 호출되는 생명주기 함수
    {
        //// 1. 힘
        //rigid.AddForce(inputVec);

        //// 2. 속도 제어
        //rigid.velocity = inputVec;

        // 3, 위치 제어
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec);
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    //void OnMove(InputValue value) inputVec.magnitude 가 적용이 안된다. 그 이유를 이따가 알아볼것.
    //{
    //    inputVec = value.Get<Vector2>();
    //}

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
