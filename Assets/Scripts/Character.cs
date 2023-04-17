using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get { return GameManager.instance.playerID == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerID == 1 ? 1.1f : 1f; }
    }

    public static float WeaponRate
    {
        get { return GameManager.instance.playerID == 1 ? 0.9f : 1f; }
    }

    public static float Damage
    {
        get { return GameManager.instance.playerID == 1 ? 1.2f : 1f; }
    }

    public static int Count
    {
        get { return GameManager.instance.playerID == 3 ? 1 : 0; }
    }
}
