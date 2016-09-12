using UnityEngine;
using System.Collections;

public class EmberStormProjectile : Projectile {    
    protected void Awake()
    {
        damage = 50;
        moveSpeed = 8.0f;
        dissipateSpeed = 0.33f;
        description = "Flaring piece of ember";
        type = "fire";
    }
}
