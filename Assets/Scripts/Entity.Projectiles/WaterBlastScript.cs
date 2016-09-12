using UnityEngine;
using System.Collections;

public class WaterBlastScript : Projectile {
    
    protected void Awake()
    {
        damage = 100;
        moveSpeed = 6.5f;
        dissipateSpeed = 0.14f;
        description = "Ball of water";
        type = "water";
    }
}
