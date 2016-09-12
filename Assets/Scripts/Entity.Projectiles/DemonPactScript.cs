using UnityEngine;
using System.Collections;

public class DemonPactScript : Projectile {
    private float creationTime;
    protected void Awake()
    {
        damage = -150;
        moveSpeed = 0.0f;
        dissipateSpeed = 0.57f;
        creationTime = Time.time;
        description = "A heal";
        type = "dark";
    }
    protected override void Update()
    {
        if(Time.time > dissipateSpeed + creationTime)
        {
            Destroy(this.gameObject);
        }
    }
}
