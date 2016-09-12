using UnityEngine;
using System.Collections;

public class HellBeamScript : Projectile {
    
	protected void Awake()
    {
        damage = 50;
        moveSpeed = 9.0f;
        dissipateSpeed = 0.17f;
        description = "Beam of demonic energy";
        type = "dark";
    }
}
