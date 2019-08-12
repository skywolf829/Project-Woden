using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	//booleans for animation and lifespan
	protected bool hit;
	protected bool facingRight;
	protected bool facingCorrectSide;
	protected bool setVelocity;

	public int damage;

	protected float moveSpeed;
	protected float dissipateSpeed;

	protected string description;    
	protected string type;

	protected Rigidbody2D rb;
	public Animator anim;


	protected virtual void Update () {
		if (!facingCorrectSide)
		{
			rb = gameObject.GetComponent<Rigidbody2D>();

			if (rb.velocity.x < 0)
			{
				if (transform.localScale.x != -1) {
					Flip ();
				}
				facingCorrectSide = true;
				if (!setVelocity) {
					rb.velocity = new Vector2 (-moveSpeed, 0);
				}
			}
			else if (rb.velocity.x > 0)
			{		
				if (transform.localScale.x != 1) {
					Flip ();
				}
				facingCorrectSide = true;
				if (!setVelocity) {
					rb.velocity = new Vector2 (moveSpeed, 0);
				}
			}
		}      
	}

	public void fixedVelocity(){
		setVelocity = true;
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
		anim = GetComponent<Animator>();
		rb = gameObject.GetComponent<Rigidbody2D> ();

		if (transform.tag == "EnemyProjectile" && c.gameObject.tag != "Enemy" 
			&& c.gameObject.tag != "Boss" && c.gameObject.tag != "EnemyProjectile" 
			&& c.gameObject.tag != "PlayerProjectile" && !hit && !c.gameObject.name.StartsWith("Battery")) {
			hit = true;
			rb.velocity = new Vector2(0, 0);
			anim.SetBool("hit", hit);
			if(c.gameObject.name == "Player"){
				c.BroadcastMessage("hitBy", type);
				c.BroadcastMessage("applyDamage", damage);
			}
			Destroy (gameObject, dissipateSpeed);
		}

		else if (transform.tag == "PlayerProjectile" && !hit 
			&& c.gameObject.tag != "Friendly" && c.gameObject.tag != "PlayerProjectile" 
			&& c.gameObject.tag != "EnemyProjectile") {
			hit = true;
			rb.velocity = new Vector2(0, 0);
			anim.SetBool("hit", hit);
			if(c.gameObject.tag == "Enemy" || c.gameObject.tag == "Boss" ){
				c.BroadcastMessage("hitBy", type);
				c.BroadcastMessage("applyDamage", damage);
                c.BroadcastMessage("damageCollisionPosition", transform.position);
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
