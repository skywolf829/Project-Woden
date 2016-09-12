using UnityEngine;
using System.Collections;

public class PrayerScript : Projectile {
    private float creationTime;
    protected void Awake()
    {
        damage = -80;
        moveSpeed = 0.0f;
        dissipateSpeed = 0.85f;
        creationTime = Time.time;
        description = "A heal";
        type = "light";
    }
    protected override void Update()
    {
        if(Time.time > dissipateSpeed + creationTime)
        {
            Destroy(this.gameObject);
        }
    }
}
