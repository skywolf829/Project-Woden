using UnityEngine;
using System.Collections;

public class SporeScript : Projectile
{
    
    protected void Awake()
    {
        damage = 20;
        moveSpeed = 7.6f;
        dissipateSpeed = 0.17f;
        description = "Ball of goop";
        type = "life";
    }
}