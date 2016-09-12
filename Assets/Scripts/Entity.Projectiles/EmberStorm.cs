using UnityEngine;
using System.Collections;

public class EmberStorm : Projectile {
    protected float creationTime;
    protected float creationLength;

    protected float duration;
    protected float lifeStart;

    protected float finishDuration;

    protected float shotInterval;
    protected float shotVariance;

    protected float nextShot;
    protected float projectileSpeed;

    protected bool creation;
    protected bool finished;

    public GameObject EmberStormProjectile;
    private GameObject player;

    protected void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        damage = 100;
        dissipateSpeed = 0.33f;
        creationLength = 0.33f;
        projectileSpeed = 8.0f;
        duration = 2.33f;
        finishDuration = 0.33f;
        shotInterval = 0.1f;
        shotVariance = 20.0f;
        description = "Ball of fire spitting out fire";
        type = "fire";
        creation = true;
        creationTime = Time.time;
        facingRight = true;
        player = GameObject.Find("Player");
    }
    protected override void Update()
    {
        if (!creation)
        {
            if(Time.time > nextShot)
            {
                createProjectile();
                nextShot = Time.time + shotInterval;
            }
            if (Time.time > lifeStart + duration)
            {
                finished = true;
                Destroy(this.gameObject, finishDuration);
            }
        }
        else
        {
            if (Time.time > creationTime + creationLength)
            {
                creation = false;
                lifeStart = Time.time;
                nextShot = Time.time;
            }
        }


        if(player.transform.position.x < this.gameObject.transform.position.x)
        {
            //Debug.Log("on right");
            if (!facingRight)
            {
                Flip();
                facingRight = true;
            }
        }
        else
        {
            //Debug.Log("on left");
            if (facingRight)
            {
                Flip();
                facingRight = false;
            }
        }
        anim.SetBool("finished", finished);
    }

    protected void createProjectile()
    {
        float r = shotVariance * (Random.value - 0.5f);
        float projectileSpeedX, projectileSpeedY;
        projectileSpeedX = projectileSpeed * Mathf.Cos(r * Mathf.Deg2Rad);
        projectileSpeedY = projectileSpeed * Mathf.Sin(r * Mathf.Deg2Rad);
        
        GameObject projectile = GameObject.Instantiate<GameObject>(EmberStormProjectile);
        Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
        if (projectile != null)
        {
            if (facingRight)
            {
                projectile.transform.position = new Vector2(transform.position.x + 0.25f, transform.position.y + 0.1f);
                projectileBody.velocity = new Vector2(projectileSpeedX, projectileSpeedY);
                projectile.BroadcastMessage("isFacingRight", facingRight);
                projectileBody.rotation = r;
            }
            else
            {
                projectile.transform.position = new Vector2(transform.position.x - 0.25f, transform.position.y + 0.1f);
                projectileBody.velocity = new Vector2(-projectileSpeedX, projectileSpeedY);
                projectile.BroadcastMessage("isFacingRight", facingRight);
                projectileBody.rotation = -r;
            }
            projectile.transform.tag = "PlayerProjectile";
        }
    }
    protected override void OnTriggerEnter2D(Collider2D c)
    {               
        if (c.gameObject.tag == "Enemy")
        {
            c.BroadcastMessage("hitBy", type);
            c.BroadcastMessage("applyDamage", damage);
        }
    }
}
