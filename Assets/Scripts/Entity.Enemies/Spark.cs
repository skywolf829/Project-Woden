using UnityEngine;
using System.Collections;

public class Spark : Enemy {

    private bool shooting;
    private bool batteryState;
    private bool batteryDeath;
	private bool up, down;

	private bool leftWall;
	private bool rightWall;
	private bool bottom;
	private bool top;

    private float batteryDeathTime;
	private float scanForBatteryTime, scanForBatteryLength;

	private Collider2D currentCollider;
    public GameObject battery, Circuit;
	private GameObject nearestBattery;

    // Use this for initialization
    protected override void Start () {
        base.Start();        
		deathAnimationLength = 0.4f;
        moveSpeed = 1.6f;
        maxSpeed = 3.0f;
        stopSpeed = 0.4f;
        maxFallSpeed = 8.0f;
        batteryDeathTime = 0.5f;
		scanForBatteryTime = Time.time;
		scanForBatteryLength = 5.0f;
        health = maxHealth = 300;
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
            left = right = up = down = false;
        }
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
				if (player.transform.position.y > transform.position.y)
				{
					up = true;
					down = false;
				}
				else
				{
					up = false;
					down = true;
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
				if (nearestBattery.transform.position.y > transform.position.y)
				{
					up = true;
					down = false;
				}
				else
				{
					up = false;
					down = true;
				}
			}
            

            if (leftWall && (up || left) && transform.position.y - collisionBox.size.y / 2 > currentCollider.transform.position.y)
            {
                leftWall = false;
                bottom = true;
                y = currentCollider.transform.position.y + collisionBox.size.y / 2 + 0.07f;
            }
            if (rightWall && (up || right) && transform.position.y - collisionBox.size.y / 2 > currentCollider.transform.position.y)
            {
                rightWall = false;
                bottom = true;
                y = currentCollider.transform.position.y + collisionBox.size.y / 2 + 0.07f;
            }

            if (leftWall && transform.position.y + collisionBox.size.y / 2 < currentCollider.transform.position.y - currentCollider.GetComponent<BoxCollider2D>().size.y)
            {
                leftWall = false;
                top = true;
                y = currentCollider.transform.position.y - currentCollider.GetComponent<BoxCollider2D>().size.y - collisionBox.size.y / 2;
            }
            if (rightWall && transform.position.y + collisionBox.size.y / 2 < currentCollider.transform.position.y - currentCollider.GetComponent<BoxCollider2D>().size.y)
            {
                rightWall = false;
                top = true;
                y = currentCollider.transform.position.y - currentCollider.GetComponent<BoxCollider2D>().size.y - collisionBox.size.y / 2;
            }

            if (bottom && (down || left) && transform.position.x + collisionBox.size.x / 2 < currentCollider.transform.position.x)
            {
                bottom = false;
                rightWall = true;
                x = currentCollider.transform.position.x - collisionBox.size.x / 2 - 0.15f;
            }
            if (bottom && (down || right) && transform.position.x - collisionBox.size.x / 2 > currentCollider.transform.position.x + currentCollider.GetComponent<BoxCollider2D>().size.x)
            {
                bottom = false;
                leftWall = true;
                x = currentCollider.transform.position.x + currentCollider.GetComponent<BoxCollider2D>().size.x + collisionBox.size.x / 2;
            }

            if(top && (up || right) && transform.position.x - collisionBox.size.x / 2 > currentCollider.transform.position.x + currentCollider.GetComponent<BoxCollider2D>().size.x)
            {
                top = false;
                leftWall = true;
                x = currentCollider.transform.position.x + currentCollider.GetComponent<BoxCollider2D>().size.x + collisionBox.size.x / 2;
            }
            if (top && (up || left) && transform.position.x + collisionBox.size.x / 2 < currentCollider.transform.position.x)
            {
                top = false;
                rightWall = true;
                x = currentCollider.transform.position.x - collisionBox.size.x / 2;
            }

            if (leftWall || rightWall)
            {
                if (up)
                {
                    dy += moveSpeed;
                    
                    if(dy > maxSpeed)
                    {
                        dy = maxSpeed;
                    }
                }
                else if (down)
                {
                    dy -= moveSpeed;
                    if(dy < -maxSpeed)
                    {
                        dy = -maxSpeed;
                    }
                }
            }
            else if(bottom || top)
            {
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
            else if(!top && !bottom && !leftWall && !rightWall)
            {
                dy = -maxFallSpeed;
            }
        
        x += Time.deltaTime * dx;
        y += Time.deltaTime * dy;
        transform.position = new Vector3(x, y, transform.position.z);
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
                }
                Destroy(gameObject);
            }
        }
        
    }
    public void upgrade(GameObject b)
    {
        if (Circuit)
        {
            GameObject c = GameObject.Instantiate(Circuit);
            c.transform.position = transform.position;
			c.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
        }
        Destroy(healthBar);
        b.BroadcastMessage("taken");
        Destroy(gameObject);        
    }
	protected override void OnCollisionEnter2D (Collision2D c)
	{
        base.OnCollisionEnter2D (c);
		if (c.gameObject.name.Equals ("RectangleObject")) {
			ContactPoint2D[] cp = c.contacts;
			double xPoint = cp [0].point.x - transform.position.x;
			double yPoint = cp [0].point.y - transform.position.y;

			for (int i = 1; i < cp.Length; i++) {			

				double xPoint2 = cp [i].point.x - transform.position.x;
				double yPoint2 = cp [i].point.y - transform.position.y;
                
				leftWall = xPoint == xPoint2 && xPoint < 0;
				rightWall = xPoint == xPoint2 && xPoint > 0;
				bottom = yPoint == yPoint2 && yPoint < 0;
				top = yPoint == yPoint2 && yPoint > 0;
				currentCollider = c.collider;
			}
            
		}
	}
    protected override void updateAnimation()
    {        
		anim.SetBool ("aggro", aggro);
		anim.SetBool ("batteryDeath", batteryDeath);
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
