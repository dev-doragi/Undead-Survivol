using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get
        {
            // return GameManager.instance.playerID == 0 ? 1.1f : 1f;
            switch (GameManager.instance.playerID)
            {
                case 0:
                    return 1.1f;
                case 4:
                    return 1.08f;
                default:
                    return 1f;
            } 
        }
    }

    public static float WeaponSpeed
    {
        get 
        {
            switch (GameManager.instance.playerID)
            {
                case 1:
                    return 1.1f;
                case 4:
                    return 1.08f;
                default:
                    return 1f;
            }
        }
    }

    public static float WeaponRate
    {
        get
        {
            switch (GameManager.instance.playerID)
            {
                case 1:
                    return 0.9f;
                case 4:
                    return 0.92f;
                default:
                    return 1f;
            }
        }
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
