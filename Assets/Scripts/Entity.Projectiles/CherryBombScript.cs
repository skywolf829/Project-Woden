using UnityEngine;
using System.Collections;

public class CherryBombScript : Projectile {
    
	protected void Awake()
    {
        damage = 35;
        moveSpeed = 7.0f;
        dissipateSpeed = 0.17f;
        description = "Ball of cherry";
        type = "dark";
    }
}
