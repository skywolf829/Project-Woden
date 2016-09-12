using UnityEngine;
using System.Collections;

public class FrostBlastPlayerScript : Projectile {    
    protected void Awake()
    {
        damage = 100;
        moveSpeed = 6.0f;
        dissipateSpeed = 0.15f;
        description = "Ball of ice";
        type = "ice";
    }
}
