using UnityEngine;
using System.Collections;

public class FireBlastScript : Projectile
{    
    protected void Awake()
    {
        damage = 100;
        moveSpeed = 7.5f;
        dissipateSpeed = 0.15f;
        description = "Ball of fire";
        type = "fire";
    }
}
