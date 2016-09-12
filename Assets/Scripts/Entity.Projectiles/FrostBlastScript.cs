using UnityEngine;
using System.Collections;

public class FrostBlastScript : Projectile {
    
	void Awake()
    {
        damage = 35;
        moveSpeed = 7.5f;
        dissipateSpeed = 0.17f;
        description = "Ball of ice";
        type = "ice";
    }	
}
