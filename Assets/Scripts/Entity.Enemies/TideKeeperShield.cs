using UnityEngine;
using System.Collections;

public class TideKeeperShield : Enemy {

	private bool expand;
	private float contractTime;

	// Use this for initialization
	protected override void Start () {
		base.Start();    
		deathAnimationLength = 0.50f;

		health = maxHealth = 1600;
		weakness = "light";
		strength = "water";
	}	

	protected override void AI()
	{
		
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
		if (!expand) {
			if (Time.time > contractTime) {
				expand = true;
			}
		}
	}

	public void timeContracted(float t)
	{
		contractTime = t + Time.time;

		expand = true;
	}

	public override void applyDamage(int d)
	{
		base.applyDamage(d);

		if (!death)
		{
			 if (health <= 0)
			{
				death = true;
				Destroy(gameObject, deathAnimationLength);
				Destroy(healthBar);
			}
		}
	}
	protected override void updateAnimation()
	{
		anim.SetBool ("death", death);
		anim.SetBool ("expanding", expand);
	}
	protected override void updateHealthBar(){
		if (!death){

			healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
			healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
		}

	}

}
