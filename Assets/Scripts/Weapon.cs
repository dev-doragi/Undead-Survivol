using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public double damage;
    public int count;
    public float speed;


    void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;
            default:

                break;
        }

        //test
        if (Input.GetButtonDown("Jump"))
            LevelUp(2f, 1);
    }

    public void LevelUp(double damage, int count)
    {
        this.damage += damage;
        this.count += count; // 무기 개수

        if (id == 0)
            Batch();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();

                break;
            default:

                break;
        }
    }

    void Batch() // 회전 무기를 count만큼 배치해주는 함수
    {
        for (int i=0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount) 
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform; // Get()은 부모가 PoolManager로 됨. 부모를 Weapon 0으로 변경
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity Per.
        }
    }
}
