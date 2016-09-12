using UnityEngine;
using System.Collections;

public class Cherry : Enemy {
    private bool charging;
    private bool enteringCacoon;
    private bool cacoon;
    private bool leavingCacoon;

    private float enteringCacoonLength;
	private float leavingCacoonLength;
	private float deathLength;
    private float regenTime;
    private float regenLength;
    private float leavingCacoonTime;
    private float enteringCacoonTime;

    private int cacoonMaxHealth;    

	// Use this for initialization
	protected override void Start () {
        base.Start();
        enteringCacoonLength = 0.25f;
		leavingCacoonLength = 0.35f;
		deathLength = 0.24f;
        moveSpeed = 0.6f;
        maxSpeed = 5.0f;
        stopSpeed = 0.2f;
        maxFallSpeed = 8.0f;
        health = maxHealth = 100;
        cacoonMaxHealth = 800;
        damage = 40;
		range = 10;
        regenLength = 8.0f;
        strength = "dark";
        weakness = "light";       
    } 

    protected override void AI()
    {
        //obtain position and velocity vectors
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
        if (death){
			return;
		}
        if (!aggro)
        {
            left = false;
            right = false;
            charging = false;
        }
        if (cacoon)
        {
            charging = false;
            if (leavingCacoon && Time.time > leavingCacoonTime + leavingCacoonLength)
            {
                leavingCacoon = false;
                cacoon = false;
                health = maxHealth;
            }
            else if(enteringCacoon && Time.time > enteringCacoonLength + enteringCacoonTime)
            {
                enteringCacoon = false;
            }
            else if (!leavingCacoon && Time.time > regenTime + regenLength)
            {
                leavingCacoon = true;
                leavingCacoonTime = Time.time;
            }
        }
        else
        {
            //face the correct direction
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
            if (aggro)
            {
                charging = true;
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
                else if(left)
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
    protected override void updateAnimation()
    {        
		anim.SetBool ("turningIntoCacoon", enteringCacoon);
        anim.SetBool("cacoon", cacoon);
        anim.SetBool("leavingCacoon", leavingCacoon);
		anim.SetBool ("charging", charging);
		anim.SetBool ("death", death);
    }
	protected override void updateHealthBar(){
		if (!death) {
            if (cacoon)
            {
                healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / cacoonMaxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
                healthBar.transform.localScale = new Vector3((float)health / cacoonMaxHealth, 1f, 1);
            }
            else
            {
                healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
                healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
            }
            
        }       
    }
    public override void applyDamage(int d){
        base.applyDamage(d);

        if (!death) {
			if(health <= 0 && !cacoon)
            {
                cacoon = true;
                enteringCacoon = true;
                enteringCacoonTime = Time.time;
                health = cacoonMaxHealth;
                regenTime = Time.time;
                rb.velocity = new Vector2(0, 0);
            }
            else if(health <= 0 && cacoon)
            {
                death = true;
                rb.velocity = new Vector2(0, 0);
                Destroy(gameObject, deathLength);
                Destroy(healthBar);
            }
		}
	}
}
