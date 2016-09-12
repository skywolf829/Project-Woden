using UnityEngine;
using System.Collections;

public class HellHound : Enemy {
    private bool shooting;
    private bool charging;
    private bool shotOnce;

    private bool walkingPhase;
    private bool shootingPhase;
    private bool chargingPhase;
    private bool jumpingPhase;
    private bool coolDownPhase;

    private float finishShoot;
    private float shootAnimationLength;
    private float shootInterval;
    private float nextShot;
    private float correctShotFrameTime;

    private float timer;
    private float chargeUpAnimationLength;
    private float coolDownAnimationLength;

    public GameObject fireball;

    public int chargeForceUp;
    public int chargeForceLateral;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        walkingPhase = true;
        shootInterval = 1.5f;
        shootAnimationLength = 0.85f;
        deathAnimationLength = 0.43f;
        correctShotFrameTime = 0.4f;
        chargeUpAnimationLength = 0.75f;
        coolDownAnimationLength = 0.42f;
        moveSpeed = 0.6f;
        maxSpeed = 3.0f;
        stopSpeed = 0.5f;
        maxFallSpeed = 8.0f;
        health = maxHealth = 2000;
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
            dx = 0;
            left = false;
            right = false;
            charging = false;
            shooting = false;
            shootingPhase = false;
            chargingPhase = false;
            walkingPhase = true;
            jumpingPhase = false;
            coolDownPhase = false;
            shotOnce = false;
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

            if (walkingPhase)
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
                if (Time.time > aggroTime + 2.0f)
                {
                    walkingPhase = false;
                    shootingPhase = true;
                    timer = Time.time;
                }
            }
            else if (shootingPhase)
            {
                shooting = true;
                dx = 0;
                if (!shotOnce)
                {
                    StartCoroutine(createProjectile(fireball, 4.0f, correctShotFrameTime));
                    shotOnce = true;
                    finishShoot = Time.time + shootAnimationLength;
                }                
            }
            else if (chargingPhase)
            {                
                charging = true;
                shotOnce = false;
                if(Time.time - timer > chargeUpAnimationLength)
                {
                    if (left) {
                        rb.AddForce(new Vector2(-chargeForceLateral, chargeForceUp));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(chargeForceLateral, chargeForceUp));

                    }
                    chargingPhase = false;
                    jumpingPhase = true;
                }
            }
            else if (jumpingPhase)
            {

            }
            else if (coolDownPhase)
            {
                if(Time.time > timer + coolDownAnimationLength + 1.0f)
                {
                    walkingPhase = true;
                    aggroTime = Time.time;
                }
            }
            
        }
        rb.velocity = new Vector2(dx, dy);
    }
    protected override void OnCollisionEnter2D(Collision2D c)
    {
        base.OnCollisionEnter2D(c);
        if(c.gameObject.name == "RectangleObject" && jumpingPhase)
        {
            Debug.Log("Done jumping");
            charging = false;
            jumpingPhase = false;
            coolDownPhase = true;
            timer = Time.time;
        }
    }
    public override void applyDamage(int d)
    {
        base.applyDamage(d);

        if (!death)
        {
            if (health <= 0)
            {
                charging = false;
                shooting = false;
                death = true;
                rb.velocity = new Vector2(0, 0);
                Destroy(gameObject, deathAnimationLength);
                Destroy(healthBar);
            }
        }
    }
    protected override void updateAnimation()
    {
        if (Time.time > finishShoot && shooting)
        {
            
            shooting = false;
            shootingPhase = false;
            if (aggro)
            {
                chargingPhase = true;
                timer = Time.time;
            }
        }
        anim.SetBool("shooting", shooting);
        anim.SetBool("charging", charging);
        anim.SetBool("death", death);
        anim.SetBool("walking", (left || right) && walkingPhase);
    }
    protected override void updateHealthBar()
    {
        if (!death)
        {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
        }

    }
}
