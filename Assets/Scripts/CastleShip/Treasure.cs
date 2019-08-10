using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField]
    private int gold;
    public int Gold
    {
        get
        {
            return gold;
        }
    }
}
