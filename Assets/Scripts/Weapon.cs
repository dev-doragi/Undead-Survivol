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

    float timer;
    Player player; // 플레이어 스크립트 자체를 컴포넌트로 쓸 수 있다.

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0: // 회전 무기
            case 5:
            case 6:
            case 7:
            case 9:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // 회전 시키는 로직
                break;
            default: // 기본 총탄 무기
                timer += Time.deltaTime;
                if (timer > speed) // 타이머가 다 돌면 총알 발사
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(double damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count; // 관통 수

        if (transform.tag == "Melee")
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero; // 플레이어 위치 기준이므로 localPosition

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i=0; i < GameManager.instance.pool.prefabs.Length; i++)
        { 
            // 
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id) // 무기 기본정보 설정 하는 로직
        {
            case 0:
            case 5:
                speed = 150 * Character.WeaponSpeed;
                transform.tag = "Melee";
                Batch();
                break;
            case 6:
            case 7:
            case 9:
                speed = 300 * Character.WeaponSpeed;
                transform.tag = "Melee";
                Batch();
                break;
            default:
                speed = 0.4f * Character.WeaponRate; // 발사 속도
                break;
        }

        // player.hands[0]는 Player.cs를 참고하여 자식 컴포넌츠 중 첫번째임
        // 따라서 Hand hand = player.hands[(int)data.itemType]; 는 
        // data.itemType이 근거리면 0, 원거리면 1 이니까,
        // player.hands[0]이 근거리, 1은 원거리가 되도록 Player 컴포넌츠의 자식의 순서를 정해야함.
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget) // 가장 가까운 적이 없을 때
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position; // 방향은 적 위치에서 내 위치를 뺀 값
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(4).transform; // 총알 위치 설정
        bullet.position = transform.position; // 플레이어 위치와 동일
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
