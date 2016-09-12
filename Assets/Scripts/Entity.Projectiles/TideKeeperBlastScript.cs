using UnityEngine;
using System.Collections;

public class TideKeeperBlastScript : Projectile {
    
	protected void Awake()
    {
        damage = 15;
        moveSpeed = 8.0f;
        dissipateSpeed = 0.17f;
        description = "Tide keeper's missile";
        type = "water";
	}
}
