using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public bool isBoth;
    public SpriteRenderer spriter; // 왜 퍼블릭일까?

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0); // 총을 들고있는 오른손
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35); // 삽을 들고있는 왼손
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);
    Quaternion bothRot = Quaternion.Euler(0, 0, -4.5f);
    Quaternion bothRotReverse = Quaternion.Euler(0, 0, 4.5f);
    Vector3 bothPos = new Vector3(0.32f, -0.31f, 0);
    Vector3 bothPosReverse = new Vector3(-0.32f, -0.31f, 0);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft) // 근접무기
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else if (isBoth)
        {
            transform.localRotation = isReverse ? bothRotReverse : bothRot;
            transform.localPosition = isReverse ? bothPosReverse : bothPos;
            spriter.flipX = isReverse;
        }
        else // 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
