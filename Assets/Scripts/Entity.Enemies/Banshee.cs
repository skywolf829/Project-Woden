using UnityEngine;
using System.Collections;

public class Banshee : Enemy {

    private bool shooting;
    private bool charging;
    private bool shotOnce;

    private float finishShoot;
	private float shootAnimationLength;
	private float shootInterval;
	private float nextShot;
	private float correctShotFrameTime;
	private float ectoplasmSpeed;

    private int maxChargeHealth;

    public GameObject Ectoplasm;

    // Use this for initialization
    protected override void Start () {
        base.Start();        
        shootInterval = 1.5f;
		shootAnimationLength = 0.35f;
		deathAnimationLength = 0.43f;
		correctShotFrameTime = 0.19f;
        moveSpeed = 0.6f;
        maxSpeed = 8.5f;
        stopSpeed = 0.1f;
        maxFallSpeed = 8.0f;
		ectoplasmSpeed = 6.0f;
        health = maxHealth = 1600;
        maxChargeHealth = 800;
        damage = 40;
		range = 10;
        weakness = "light";
        strength = "dark";
    }	

    protected override void AI()
    {
        x = tran.position.x;
        y = tran.position.y;
        dx = rb.velocity.x;
        dy = rb.velocity.y;
        if (shielded)
        {
            if (shield != null)
            {
                shield.transform.position = new Vector2(x, y);
            }
            else
            {
                shielded = false;
            }

        }
        if (death)
        {
            return;
        }
        if (!aggro)
        {
            left = false;
            right = false;
            charging = false;
            shooting = false;
        }
        else
        {
            if (player.transform.position.x > x)
            {
                right = true;
                left = false;
                if (!facingRight)
                {
                    Flip();
                }
            }
            else
            {
                left = true;
                right = false;
                if (facingRight)
                {
                    Flip();
                }
            }

            if (!charging && !shooting)
            {
                if (Time.time > nextShot)
                {
                    StartCoroutine(createProjectile(Ectoplasm, ectoplasmSpeed, correctShotFrameTime));
                    shooting = true;
                    finishShoot = Time.time + shootAnimationLength;
                }
            }
            if (charging)
            {
                if (right)
                {
                    if (dx < 0)
                    {
                        dx += stopSpeed;
                    }
                    else
                    {
                        dx += moveSpeed;
                        if (dx > maxSpeed)
                        {
                            dx = maxSpeed;
                        }
                    }
                }
                else
                {
                    if (dx > 0)
                    {
                        dx -= stopSpeed;
                    }
                    else
                    {
                        dx -= moveSpeed;
                        if (dx < -maxSpeed)
                        {
                            dx = -maxSpeed;
                        }
                    }
                }
            }
        }
        rb.velocity = new Vector2(dx, dy);
    }
    public override void applyDamage(int d)
    {
        base.applyDamage(d);

        if (!death)
        {
            if (health <= 0 && !charging)
            {
                charging = true;
                shooting = false;
                health = maxChargeHealth;
            }
            else if (health <= 0 && charging)
            {
                charging = false;
                death = true;
                rb.velocity = new Vector2(0, 0);
                Destroy(gameObject, deathAnimationLength);
                Destroy(healthBar);
            }
        }
    }
    protected override void updateAnimation()
    {
        if (Time.time > finishShoot && shooting) {
			shooting = false;
			nextShot = Time.time + shootInterval;
		}
		anim.SetBool ("shooting", shooting);
		anim.SetBool ("charging", charging);
		anim.SetBool ("death", death);
    }
	protected override void updateHealthBar(){
		if (death) {
			return;
		}
        if (charging)
        {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxChargeHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
            healthBar.transform.localScale = new Vector3((float)health / maxChargeHealth, 1f, 1);
        }
        else
        {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
        }
		
	}
    
}
