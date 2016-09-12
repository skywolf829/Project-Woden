using UnityEngine;
using System.Collections;

public class Charge : Enemy {

    private bool shooting;
    private bool batteryState;
    private bool attacking;

    private float boltSpeed;
    private float startTimer;
    private float shootInterval, shootAnimationLength;
	private float scanForBatteryTime, scanForBatteryLength;

	private Collider2D currentCollider;
    public GameObject battery, electricBolt;
	private GameObject nearestBattery;

    // Use this for initialization
    protected override void Start () {
        base.Start();        
		deathAnimationLength = 0.4f;
        shootInterval = 3.0f;
        shootAnimationLength = 0.35f;

        boltSpeed = 15.0f;
        moveSpeed = 1.6f;
        maxSpeed = 3.0f;
        stopSpeed = 0.4f;
		maxFallSpeed = 8.0f;
		scanForBatteryTime = Time.time;
		scanForBatteryLength = 5.0f;
        health = maxHealth = 600;
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
            if (attacking)
            {
                if(Time.time - startTimer > shootAnimationLength * 2)
                {
                    attacking = false;
                    startTimer = Time.time;
                }
            }
            else
            {
				if (nearestBattery) {
					if (left)
					{
						if (dx > 0)
						{
							dx -= stopSpeed;
						}
						else
						{
							dx -= moveSpeed;
						}
						if(dx < -maxSpeed)
						{
							dx = -maxSpeed;
						}
					}
					else if (right)
					{
						if (dx < 0)
						{
							dx += stopSpeed;
						}
						else
						{
							dx += moveSpeed;
						}
						if(dx > maxSpeed)
						{
							dx = maxSpeed;
						}
					}
				}
                else if (Time.time - startTimer > shootInterval)
                {
                    attacking = true;
                    startTimer = Time.time;
                    if (electricBolt)
                    {
                        StartCoroutine(createPlayerLockedProjectile(electricBolt, boltSpeed, shootAnimationLength));
                        StartCoroutine(createPlayerLockedProjectile(electricBolt, boltSpeed, shootAnimationLength * 2));
                    }
                }
            }
        }
        rb.velocity = new Vector2(dx, dy);
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
				damage = 0;
                Destroy(healthBar);
                if (battery)
                {
                    GameObject b = Instantiate(battery);
                    b.transform.position = transform.position;
                    GameObject b2 = Instantiate(battery);
                    b2.transform.position = transform.position + new Vector3(0.5f, 0, 0);
                }
                Destroy(gameObject);
            }
        }
        
    }
    public void upgrade(GameObject b)
    {
        applyDamage(-200);
        b.BroadcastMessage("taken");
    }

    protected override void OnTriggerEnter2D(Collider2D c)
    {
        base.OnTriggerEnter2D(c);
        if (death && c.gameObject.name == "Player")
        {
            if (battery)
            {
                GameObject.FindGameObjectWithTag("Inventory").BroadcastMessage("addItem", battery);
                GameObject.FindGameObjectWithTag("Inventory").BroadcastMessage("addItem", battery);
            }
            Destroy(gameObject);
        }
    }
    protected override void updateAnimation()
    {        
		anim.SetBool ("aggro", aggro);
		anim.SetBool ("attacking", attacking);
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
