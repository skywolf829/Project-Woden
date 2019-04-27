using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallScript : EnemyProjectile
{
    float speed = 4;
    float dx, dy;

    public void SetTarget(Vector3 target)
    {
        rb = GetComponent<Rigidbody2D>();
        float angleInDeg, projectileSpeedX, projectileSpeedY;
        angleInDeg = Mathf.Atan((target.y - transform.position.y) / (target.x - transform.position.x)) * Mathf.Rad2Deg;
        projectileSpeedX = speed * Mathf.Cos(angleInDeg * Mathf.Deg2Rad);
        projectileSpeedY = speed * Mathf.Sin(angleInDeg * Mathf.Deg2Rad);
        if (target.x < transform.position.x)
        {
            projectileSpeedY *= -1;
        }
        transform.tag = "EnemyProjectile";
        rb.velocity = new Vector3(target.x < transform.position.x ? -projectileSpeedX: projectileSpeedX, projectileSpeedY);        
    }
    // Update is called once per frame
    protected override void Update()
    {
           
    }
    protected virtual void OnTriggerEnter2D(Collider2D c)
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (c.gameObject.tag != "Enemy" && c.gameObject.tag != "Boss" && c.gameObject.tag != "Projectile" && 
            c.gameObject.tag != "EnemyProjectile" && c.gameObject.tag != "PlayerProjectile" && !hit)
        {
            hit = true;
            rb.velocity = new Vector2(0, 0);
            anim.SetTrigger("hit");
            if (c.gameObject.name == "Player")
            {
                c.BroadcastMessage("hitBy", type);
                c.BroadcastMessage("applyDamage", damage);
            }
        }
    }
    public void TriggerDestroy()
    {
        Destroy(gameObject);
    }
}
