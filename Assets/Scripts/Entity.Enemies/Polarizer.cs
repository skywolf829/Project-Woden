using UnityEngine;
using System.Collections;

public class Polarizer : Enemy {

	private bool channeling;

	private float startTimer;
	private float channelInterval, channelLength;
	private float scanForBatteryTime, scanForBatteryLength;

	public GameObject battery;
	private GameObject nearestBattery;

	// Use this for initialization
	protected override void Start () {
		base.Start();        
		deathAnimationLength = 0.4f;
		channelInterval = 3.0f;
		channelLength = 5.0f;

		moveSpeed = 1.6f;
		maxSpeed = 3.0f;
		stopSpeed = 0.4f;
		maxFallSpeed = 8.0f;
		scanForBatteryTime = Time.time;
		scanForBatteryLength = 5.0f;
		health = maxHealth = 800;
		damage = 35;
		range = 10;
		weakness = "water";
		strength = "electric";
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

		if (Time.time > scanForBatteryTime) {
			nearestBattery = scanForBattery ();
			scanForBatteryTime += scanForBatteryLength;
		}

		if (!aggro)
		{
			left = right = false;
		}
		else
		{
			if (!nearestBattery) {
				if (player.transform.position.x < transform.position.x) {
					left = true;
					right = false;
					if (facingRight) {
						Flip ();
					}
				} else {
					right = true;
					left = false;
					if (!facingRight) {
						Flip ();
					}
				}
			} else {
				if (nearestBattery.transform.position.x < transform.position.x) {
					left = true;
					right = false;
					if (facingRight) {
						Flip ();
					}
				} else {
					right = true;
					left = false;
					if (!facingRight) {
						Flip ();
					}
				}
			}

			if(startTimer == 0)
			{
				startTimer = Time.time;
			}
			if (channeling)
			{
				if(Time.time - startTimer > channelLength)
				{
					channeling = false;
					startTimer = Time.time;
				}
			}
			else
			{
				if (nearestBattery) {
					if (left) {
						if (dx > 0) {
							dx -= stopSpeed;
						} else {
							dx -= moveSpeed;
						}
						if (dx < -maxSpeed) {
							dx = -maxSpeed;
						}
					} else if (right) {
						if (dx < 0) {
							dx += stopSpeed;
						} else {
							dx += moveSpeed;
						}
						if (dx > maxSpeed) {
							dx = maxSpeed;
						}
					}
				} else if (Time.time - startTimer > channelInterval) {
					dx = 0;
					channeling = true;
					startTimer = Time.time;
				} else {
					dx = 0;
				}
			}
		}
		rb.velocity = new Vector2(dx, dy);
	}
	public override void applyDamage(int d)
	{
		if (!channeling) {
			base.applyDamage (d);
		}
		if (!death)
		{
			if (health <= 0)
			{
				death = true;            
				rb.velocity = new Vector2(0, 0);
				damage = 0;
				Destroy(healthBar);
				if (battery)
				{
					GameObject b = Instantiate(battery);
					b.transform.position = transform.position;
					GameObject b2 = Instantiate(battery);
					b2.transform.position = transform.position + new Vector3(0.5f, 0, 0);
					GameObject b3 = Instantiate(battery);
					b3.transform.position = transform.position + new Vector3(1.0f, 0, 0);
				}
				Destroy(gameObject);
			}
		}

	}


	protected override void OnTriggerEnter2D(Collider2D c)
	{
		base.OnTriggerEnter2D(c);
		if (channeling && c.gameObject.tag == "PlayerProjectile") {
			StartCoroutine (createProjectile(c.gameObject, 5.0f, 0));
		}
		if (death && c.gameObject.name == "Player")
		{
			if (battery)
			{
				GameObject.FindGameObjectWithTag("Inventory").BroadcastMessage("addItem", battery);
				GameObject.FindGameObjectWithTag("Inventory").BroadcastMessage("addItem", battery);
				GameObject.FindGameObjectWithTag("Inventory").BroadcastMessage("addItem", battery);
			}
			Destroy(gameObject);
		}
	}
	protected override void updateAnimation()
	{        
		anim.SetBool ("channeling", channeling);
		anim.SetBool ("death", death);
	}
	protected override void updateHealthBar(){
		if (death) {
			return;
		}
		else
		{
			healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
			healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
		}

	}

}
