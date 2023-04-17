using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner; // Scanner라는 스크립트를 컴포넌트로 사용
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

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

    void OnEnable()
    {
        speed *= Character.Speed; // Speed.cs에서 속성을 가져옴
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerID];
    }

    void FixedUpdate() // 물리 연산 프레임마다 호출되는 생명주기 함수
    {
        if (!GameManager.instance.isLive)
            return;
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
        if (!GameManager.instance.isLive)
            return;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    //void OnMove(InputValue value) inputVec.magnitude 가 적용이 안된다. 그 이유를 이따가 알아볼것.
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0) // 체력이 모두 닳으면
        {
            for (int i=3; i< transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                // Player 컴포넌트의 Area부터 자식 오브젝트들 비활성화
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
