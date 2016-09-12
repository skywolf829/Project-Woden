using UnityEngine;
using System.Collections;

public class EctoplasmScript : Projectile {
    
	protected void Awake()
    {
        damage = 35;
        moveSpeed = 7.5f;
        dissipateSpeed = 0.17f;
        description = "Ectoplasm";
        type = "dark";
    }
}
