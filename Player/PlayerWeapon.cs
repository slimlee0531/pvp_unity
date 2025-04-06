using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerWeapon
{
    public string name = "AK47";
    public float damage = 10f;
    public float range = 100f;
    public float shootRate = 3f;
    public GameObject graphics;
}
