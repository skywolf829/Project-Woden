using UnityEngine;
using System.Collections;

public class FrostMagus : Enemy {    
    private bool hasShield;
    private bool shooting;
    private bool walking;
    private bool shotOnce;
    private bool waiting;

    private float finishShoot;
	private float shootAnimationLength;
	private float shootInterval;
	private float nextShot;
    private float startWaitTime;
    private float waitTime;
	private float correctShotFrameTime;
	private float frostBlastSpeed;
    private float iceBlastSpeed;

    private int shotCount;
    
    public GameObject FrostBlast;
    public GameObject IceBlast;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        shootInterval = 2.4f;
		shootAnimationLength = 0.5f;
		deathAnimationLength = 0.34f;
		correctShotFrameTime = 0.4f;
        waitTime = 1.3f;
        moveSpeed = 0.5f;
        maxSpeed = 0.5f;
        stopSpeed = 0.3f;
        maxFallSpeed = 8.0f;
		iceBlastSpeed = 6.0f;
        frostBlastSpeed = 5.5f;
        health = maxHealth = 1000;
        damage = 10;
		range = 10;
        weakness = "fire";
        strength = "ice"; 
    }
    

    protected override void AI()
    {
        x = tran.position.x;
        y = tran.position.y;

        dy = rb.velocity.y;
        dx = rb.velocity.x;
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
        if (death){
			return;
		}
		if(!aggro){
			left = false;
			right = false;
            walking = false;
            shotCount = 0;
		}
		else{
			if(player.transform.position.x > x){
				right = true;
				left = false;
				if(!facingRight){
					Flip ();
				}
			}
			else{
				left = true;
				right = false;
				if(facingRight){
					Flip ();
				}
			}
            if (waiting && Time.time > startWaitTime + waitTime)
            {
                waiting = false;
                walking = true;
            }

			if(!shooting){
				if(Time.time > nextShot && shotCount % 2 != 1){
					StartCoroutine(createProjectile(FrostBlast, frostBlastSpeed, correctShotFrameTime));
					shooting = true;
					finishShoot = Time.time + shootAnimationLength;
                    walking = false;
                    shotCount++;
				}
                else if(Time.time > nextShot)
                {
                    StartCoroutine(createProjectile(IceBlast, iceBlastSpeed, correctShotFrameTime));
                    shooting = true;
                    finishShoot = Time.time + shootAnimationLength;
                    walking = false;
                    shotCount++;
                }
			}
			if(walking){
				if(right){
					if(dx < 0){
                        dx += stopSpeed;
					}
					else{
                        dx += moveSpeed;
						if(dx > maxSpeed){
                            dx = maxSpeed;
						}
					}
				}
				else{
					if(dx > 0){
                        dx -= stopSpeed;
					}
					else{
                        dx -= moveSpeed;
						if(dx < -maxSpeed){
                            dx = -maxSpeed;
						}
					}
				}
			}    		
		}

        rb.velocity = new Vector2(dx, dy);
        tran.position = new Vector2(x, y);
    }
    
    protected override void updateAnimation()
    {
        if (Time.time > finishShoot && shooting) {
			shooting = false;
			nextShot = Time.time + shootInterval;
            startWaitTime = Time.time;
            waiting = true;
		}
		anim.SetBool ("shooting", shooting);
		anim.SetBool ("walking", walking);
		anim.SetBool ("death", death);
    }
	protected override void updateHealthBar(){
		if (!death) {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
        }        
    }
	public override void applyDamage(int d){
        base.applyDamage(d);

		if (!death) {
			if(health <= 0){
				death = true;
				rb.velocity = new Vector2(0, 0);
				Destroy (gameObject, deathAnimationLength);
				Destroy (healthBar);
			}
		}
	}
}
