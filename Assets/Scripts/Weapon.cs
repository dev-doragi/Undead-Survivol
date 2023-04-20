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
    Player player; // �÷��̾� ��ũ��Ʈ ��ü�� ������Ʈ�� �� �� �ִ�.

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
            case 0: // ȸ�� ����
            case 5:
            case 6:
            case 7:
            case 9:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // ȸ�� ��Ű�� ����
                break;
            default: // �⺻ ��ź ����
                timer += Time.deltaTime;
                if (timer > speed) // Ÿ�̸Ӱ� �� ���� �Ѿ� �߻�
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
        this.count += count; // ���� ��

        if (transform.tag == "Melee")
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero; // �÷��̾� ��ġ �����̹Ƿ� localPosition

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

        switch (id) // ���� �⺻���� ���� �ϴ� ����
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
                speed = 0.4f * Character.WeaponRate; // �߻� �ӵ�
                break;
        }

        // player.hands[0]�� Player.cs�� �����Ͽ� �ڽ� �������� �� ù��°��
        // ���� Hand hand = player.hands[(int)data.itemType]; �� 
        // data.itemType�� �ٰŸ��� 0, ���Ÿ��� 1 �̴ϱ�,
        // player.hands[0]�� �ٰŸ�, 1�� ���Ÿ��� �ǵ��� Player ���������� �ڽ��� ������ ���ؾ���.
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch() // ȸ�� ���⸦ count��ŭ ��ġ���ִ� �Լ�
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
                bullet.parent = transform; // Get()�� �θ� PoolManager�� ��. �θ� Weapon 0���� ����
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
        if (!player.scanner.nearestTarget) // ���� ����� ���� ���� ��
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position; // ������ �� ��ġ���� �� ��ġ�� �� ��
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(4).transform; // �Ѿ� ��ġ ����
        bullet.position = transform.position; // �÷��̾� ��ġ�� ����
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
