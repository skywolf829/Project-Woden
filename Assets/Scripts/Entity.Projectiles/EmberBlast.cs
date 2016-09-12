using UnityEngine;
using System.Collections;

public class EmberBlast : Projectile {
    
    protected void Awake()
    {
        damage = 70;
        moveSpeed = 8.0f;
        dissipateSpeed = 0.17f;
        description = "Ember";
        type = "fire";
    }
}
