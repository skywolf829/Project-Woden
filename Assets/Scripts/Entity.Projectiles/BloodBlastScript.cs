using UnityEngine;
using System.Collections;

public class BloodBlastScript : Projectile {   	
    void Awake()
    {
        damage = 35;
        moveSpeed = 7.5f;
        dissipateSpeed = 0.17f;
        description = "Blast of blood";
        type = "water";
    }
}
