using UnityEngine;
using System.Collections;

public class HellOrbScript : Projectile {
	
	protected void Awake()
    {
        damage = 40;
        moveSpeed = 5.0f;
        dissipateSpeed = 0.17f;
        description = "Ball of demonic energy";
        type = "dark";
    }
}
