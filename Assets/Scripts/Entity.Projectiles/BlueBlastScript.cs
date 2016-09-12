using UnityEngine;
using System.Collections;

public class BlueBlastScript : Projectile
{
    protected void Awake()
    {
        damage = 35;
        moveSpeed = 7.0f;
        dissipateSpeed = 0.17f;
        description = "A blast of blue?";
        type = "water";
    }
}