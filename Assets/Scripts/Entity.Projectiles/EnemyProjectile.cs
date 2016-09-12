using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

	//booleans for animation and lifespan
	protected bool hit;
    protected bool facingRight;
    protected bool facingCorrectSide;
	
	public int damage;

    protected float moveSpeed;
    protected float dissipateSpeed;

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
        }		
	}
	
	protected virtual void OnTriggerEnter2D(Collider2D c){
		anim = GetComponent<Animator>();
		rb = gameObject.GetComponent<Rigidbody2D> ();
		if (c.gameObject.tag != "Enemy" && c.gameObject.tag != "Boss" && c.gameObject.tag != "Projectile" && !hit) {
			hit = true;
			rb.velocity = new Vector2(0, 0);
			anim.SetBool("hit", hit);
			if(c.gameObject.name == "Player"){
                c.BroadcastMessage("hitBy", type);
                c.BroadcastMessage("applyDamage", damage);
			}
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
