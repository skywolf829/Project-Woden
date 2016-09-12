using UnityEngine;
using System.Collections;

public class FateJavelineScript : Projectile {
    protected float creationTime;
    protected float creationLength;

    protected bool creation;
    protected bool moving;

    protected void Awake()
    {
        damage = 100;
        moveSpeed = 10.0f;
        dissipateSpeed = 0.33f;
        creationLength = 0.45f;
        description = "Bolt of light";
        type = "light";
        creation = true;
        creationTime = Time.time;
    }
    protected override void Update()
    {
        if (!creation && !moving)
        {
            if (facingRight)
            {
                rb.velocity = new Vector2(moveSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, 0);
            }
            moving = true;
        }
        else
        {
            if (Time.time > creationTime + creationLength)
            {
                creation = false;
            }
        }

    }
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if(!hit && (c.gameObject.name == "RectangleObject" || c.gameObject.tag == "Boundary"))
        {
            hit = true;
            rb.velocity = new Vector2(0, 0);
            anim.SetBool("hit", hit);
            Destroy(gameObject, dissipateSpeed);
        }
        else if (!hit && c.gameObject.tag == "Enemy")
        {
            c.BroadcastMessage("hitBy", type);
            c.BroadcastMessage("applyDamage", damage);
        }
    }
}
