using UnityEngine;
using System.Collections;

public class LavaBlastScript : Projectile {
    
	protected void Awake()
    {
        damage = 60;
        moveSpeed = 7.0f;
        dissipateSpeed = 0.17f;
        description = "Lava";
        type = "fire";
    }
}
