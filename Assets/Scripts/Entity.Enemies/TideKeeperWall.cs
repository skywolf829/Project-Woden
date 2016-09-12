using UnityEngine;
using System.Collections;

public class TideKeeperWall : Enemy {


	private float hitTime;
	private float hitAnimationLength;


	// Use this for initialization
	protected override void Start () {
		base.Start ();
		hitAnimationLength = 0.33f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (hitTime != 0 && Time.time > hitTime + hitAnimationLength) {
			anim.ResetTrigger ("Hit");
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D c)
	{        
		if (c.gameObject.name == "Player")
		{
			Rigidbody2D r = c.gameObject.GetComponent<Rigidbody2D> ();
			if (transform.position.x < c.gameObject.transform.position.x) {
				if (r) {
					r.velocity = new Vector2 (20, r.velocity.y);
				}
			} else {
				if (r) {
					r.velocity = new Vector2 (-20, r.velocity.y);
				}
			}

			anim.SetTrigger("Hit");
			hitTime = Time.time;
		}       

	}
}
