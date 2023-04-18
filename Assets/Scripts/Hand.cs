using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public bool isBoth;
    public SpriteRenderer spriter; // �� �ۺ��ϱ�?

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0); // ���� ����ִ� ������
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35); // ���� ����ִ� �޼�
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

        if (isLeft) // ��������
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
        else // ���Ÿ� ����
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
