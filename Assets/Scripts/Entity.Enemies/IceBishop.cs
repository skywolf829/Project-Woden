using UnityEngine;
using System.Collections;

public class IceBishop : Enemy
{
    private bool shooting;
    private bool walking;
    private bool shotOnce;
    private bool waiting;

    private float finishShoot;
    private float shootAnimationLength;
    private float shootInterval;
    private float nextShot;
    private float startWaitTime;
    private float waitTime;
    private float correctShotFrameTime;
    private float frostBlastSpeed;
    private float iceBlastSpeed;
    
    private int shotCount;   
    
    public GameObject FrostBlast;
    public GameObject heal;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        shootInterval = 2.4f;
        shootAnimationLength = 0.5f;
        deathAnimationLength = 0.34f;
        correctShotFrameTime = 0.4f;
        waitTime = 1.3f;
        moveSpeed = 0.7f;
        maxSpeed = 0.7f;
        stopSpeed = 0.3f;
        maxFallSpeed = 8.0f;
        iceBlastSpeed = 6.0f;
        frostBlastSpeed = 5.5f;
        health = maxHealth = 1000;
        damage = 10;
        range = 10;
        strength = "ice";
        weakness = "fire";
    }
    

    protected override void AI()
    {
        x = tran.position.x;
        y = tran.position.y;

        dy = rb.velocity.y;
        dx = rb.velocity.x;
        if (shielded)
        {
            if(shield != null)
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
            walking = false;
            shotCount = 0;
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
            if (waiting && Time.time > startWaitTime + waitTime)
            {
                waiting = false;
                walking = true;
            }

            if (!shooting)
            {
                if (Time.time > nextShot && shotCount % 3 != 1)
                {
                    StartCoroutine(createProjectile(FrostBlast, frostBlastSpeed, correctShotFrameTime));
                    shooting = true;
                    finishShoot = Time.time + shootAnimationLength;
                    walking = false;
                    shotCount++;
                }
                else if (Time.time > nextShot)
                {
                    StartCoroutine(createHeal(correctShotFrameTime));
                    shooting = true;
                    finishShoot = Time.time + shootAnimationLength;
                    walking = false;
                    shotCount++;
                }
            }
            if (walking)
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
        tran.position = new Vector2(x, y);
    }
    private IEnumerator createHeal(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy");
        ArrayList enemiesInRange = new ArrayList();

        for(int i = 0; i < enemies2.Length; i++)
        {
            if (Mathf.Abs(tran.position.x - enemies2[i].transform.position.x) < 5 &&
                Mathf.Abs(tran.position.y - enemies2[i].transform.position.y) < 5)
            {
                enemiesInRange.Add(enemies2[i]);
            }
        }
        if (enemiesInRange.Count > 0)
        {
            int rand = (int)(Random.value * enemiesInRange.Count);
            GameObject chosen = enemiesInRange[rand] as GameObject;

            GameObject projectile = GameObject.Instantiate<GameObject>(heal);
            if (projectile != null)
            {                
                projectile.transform.position = new Vector2(chosen.transform.position.x, chosen.transform.position.y);             
            }
        }
        
    }
    
    protected override void updateAnimation()
    {
        if (Time.time > finishShoot && shooting)
        {
            shooting = false;
            nextShot = Time.time + shootInterval;
            startWaitTime = Time.time;
            waiting = true;
        }
        anim.SetBool("shooting", shooting);
        anim.SetBool("walking", walking);
        anim.SetBool("death", death);
    }
    public override void applyDamage(int d)
    {
        base.applyDamage(d);
        
        if (!death)
        {
            if (health <= 0)
            {
                death = true;
                rb.velocity = new Vector2(0, 0);
                Destroy(gameObject, deathAnimationLength);
                Destroy(healthBar);
            }
        }
    }
}