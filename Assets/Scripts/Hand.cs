using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter; // �� �ۺ��ϱ�?
    Player players;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0); // ���� ����ִ� ������
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35); // ���� ����ִ� �޼�
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
        players = GameManager.instance.player;
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
        else if (players.scanner.nearestTarget) // ���⼭���� ����
        {
            if (isReverse)
            {
                transform.localPosition = rightPos;
                spriter.flipX = true;
                Vector3 targetPos = players.scanner.nearestTarget.position;
                Vector3 dir = targetPos - transform.position;
                transform.localRotation = Quaternion.FromToRotation(Vector3.right, dir);
            }
            else
            {
                transform.localPosition = rightPosReverse;
                spriter.flipX = false;
                Vector3 targetPos = players.scanner.nearestTarget.position;
                Vector3 dir = targetPos - transform.position;
                transform.localRotation = Quaternion.FromToRotation(Vector3.right, dir);
            }

            bool isRotA = transform.localRotation.eulerAngles.z > 90 && transform.localRotation.eulerAngles.z < 270;
            bool isRotB = transform.localRotation.eulerAngles.z > -90 && transform.localRotation.eulerAngles.z < -270;
            spriter.flipY = isRotA || isRotB;
        }
        else // ���Ÿ� ����
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 6;
        }
    }
}
