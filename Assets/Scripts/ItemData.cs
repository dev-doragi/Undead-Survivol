using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")] // 레벨 별 능력치
    public float baseDamage; // 기본 데미지
    public int baseCount; // 기본 관통력
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")] // 무기
    public GameObject projectile;
    public Sprite hand;
}
