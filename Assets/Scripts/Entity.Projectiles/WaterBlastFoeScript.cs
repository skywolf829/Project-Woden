using UnityEngine;
using System.Collections;

public class WaterBlastFoeScript : Projectile {
    
	protected void Awake()
    {
        damage = 20;
        moveSpeed = 6.0f;
        dissipateSpeed = 0.17f;
        description = "Ball of water";
        type = "water";
    }
	
}
