using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public double damage;
    public int per;

    public void Init(double damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
