using UnityEngine;
using System.Collections;

public class LightBlastScript : Projectile {
    
    protected void Awake()
    {
        damage = 100;
        moveSpeed = 7.5f;
        dissipateSpeed = 0.15f;
        description = "Bolt of light";
        type = "light";
    }
}
