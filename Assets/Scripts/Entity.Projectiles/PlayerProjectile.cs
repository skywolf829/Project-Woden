using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
    
    protected bool hit;
	protected bool facingRight;
    protected bool facingCorrectSide;

    protected float moveSpeed;
    protected float dissipateSpeed;

    protected int damage;
    
	protected string description;    
	protected string type;
    
    protected Rigidbody2D rb;
    protected Animator anim;
    	
	protected virtual void Update () {
        if (!facingCorrectSide)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

            if (rb.velocity.x < 0)
            {
                Flip();
                facingCorrectSide = true;
            }
            else if (rb.velocity.x > 0)
            {
                facingCorrectSide = true;
            }
            else if (rb.velocity.x == 0)
            {
                GameObject Player = GameObject.Find("Player");
                Vector2 playerPos = Player.transform.position;
                if (rb.position.x < playerPos.x)
                {
                    Flip();
                    facingCorrectSide = true;
                }
            }
        }      
	}

    public void isFacingRight(bool b)
    {
        facingRight = b;
        facingCorrectSide = true;
        if (!facingRight)
        {
            Flip();
        }
    }

	protected virtual void OnTriggerEnter2D(Collider2D c){
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (!hit && c.gameObject.tag != "Friendly" && c.gameObject.tag != "Projectile") {
			hit = true;
            rb.velocity = new Vector2(0, 0);
			if(c.gameObject.tag == "Enemy"){
                c.BroadcastMessage("hitBy", type);
                c.BroadcastMessage("applyDamage", damage);
			}
            anim.SetBool("hit", hit);
            Destroy (gameObject, dissipateSpeed);
		}
	}

	protected void Flip()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
