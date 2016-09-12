using UnityEngine;
using System.Collections;

public class IceSpikeScript : Projectile {

    private float creationLength = 0.8f;
    private float creationTime;
    private bool beingCreated;

    void Awake()
    {
        creationTime = Time.time;
        beingCreated = true;
        damage = 35;
        moveSpeed = 6.0f;
        dissipateSpeed = 0.17f;
        description = "Sharp ice spike";
        type = "ice";
    }
    
    protected override void Update () {
		
        if (beingCreated && !facingCorrectSide)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb.velocity.x > 0)
            {
                facingRight = true;
                rb.velocity = new Vector2(0, 0);
                facingCorrectSide = true;
                Flip();
            }
            else
            {
                facingRight = false;
                rb.velocity = new Vector2(0, 0);
                facingCorrectSide = true;
            }
        }
        else if (beingCreated && Time.time > creationLength + creationTime)
        {
            beingCreated = false;
        }
        else if (!beingCreated && !hit)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            if (facingRight)
            {
                rb.velocity = new Vector2(moveSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, 0);
            }
        }
	}
}
