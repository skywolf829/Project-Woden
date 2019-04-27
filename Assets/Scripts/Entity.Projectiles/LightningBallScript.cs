using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallScript : EnemyProjectile
{
    float speed = 2;
    float dx, dy;

    public void SetTarget(Vector3 target)
    {
        float angleInDeg, projectileSpeedX, projectileSpeedY;
        angleInDeg = Mathf.Atan((target.y - transform.position.y) / (target.x - transform.position.x)) * Mathf.Rad2Deg;
        projectileSpeedX = speed * Mathf.Cos(angleInDeg * Mathf.Deg2Rad);
        projectileSpeedY = speed * Mathf.Sin(angleInDeg * Mathf.Deg2Rad);
        if (target.x < transform.position.x)
        {
            projectileSpeedY *= -1;
        }
        transform.tag = "EnemyProjectile";
        transform.Rotate(new Vector3(angleInDeg, 0, 0));
        rb.velocity = new Vector3(projectileSpeedX, projectileSpeedY);        
    }
    // Update is called once per frame
    protected override void Update()
    {
        
    }
    protected virtual void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag != "Enemy" && c.gameObject.tag != "Boss" && c.gameObject.tag != "Projectile" && !hit)
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
