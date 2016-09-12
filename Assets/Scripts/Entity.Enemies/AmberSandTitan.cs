using UnityEngine;
using System.Collections;

public class AmberSandTitan : Enemy {
    private bool stomp1, stomp2;
    private bool glassDeath;
    private bool glassForm;
    private bool takeGlass;
    private bool shotOnce;
    private bool finishStomp1, finishStomp2;
    private bool shooting;
    private bool directionChosen;
    private bool committedRight;

    private float shootAnimationLength, shootTime;
    private float finishStomp1Time, finishStomp2Time;
	private float stompAnimationLength;
	private float stompInterval;    
    private float glassDeathAnimationLength;
    private float takeGlassAnimationLength;
    private float stompTime;
    private float emberBlastSpeed;
    private float emberBlastTime;

    private int lastStomp;

    public GameObject SandSpike;
    public GameObject EmberBlast;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        stompInterval = 1.0f;
		stompAnimationLength = 0.6f;
		deathAnimationLength = 0.33f;
        glassDeathAnimationLength = 0.4f;
        shootAnimationLength = 1.5f;
        moveSpeed = 0.1f;
        maxSpeed = 1.1f;
        emberBlastSpeed = 9.0f;
        emberBlastTime = 0.2f;
        health = maxHealth = 1500;
        damage = 50;
		range = 10;
        strength = "sand";
        weakness = "fire";    
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
        if (!death && !glassDeath && !glassForm && !takeGlass){
            if (!aggro)
            {
                left = false;
                right = false;
                stomp1 = false;
                stomp2 = false;
                finishStomp1 = false;
                finishStomp2 = false;
                directionChosen = false;
                shooting = false;
                shotOnce = false;
            }
            else
            {
                if (player.transform.position.x > x)
                {
                    right = true;
                    left = false;
                    if (!facingRight && !directionChosen)
                    {
                        Flip();
                    }
                }
                else
                {
                    left = true;
                    right = false;
                    if (facingRight && !directionChosen)
                    {
                        Flip();
                    }
                }

                /*
                 * Need to finish AI code
                 */
            }                
        }
		
        rb.velocity = new Vector2(dx, dy);
    }
    protected override void updateAnimation()
    {        
        anim.SetBool("stomp1", stomp1);
        anim.SetBool("stomp2", stomp2);
		anim.SetBool ("glassDeath", glassDeath);
		anim.SetBool ("takeGlass", takeGlass);
		anim.SetBool ("death", death);
    }
	protected override void updateHealthBar(){
		if (!death && !glassDeath && !glassForm && !takeGlass) {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
        }        
	}
	public override void applyDamage(int d){
        base.applyDamage(d);

        if (!death && !glassDeath && !glassForm && !takeGlass) {
			if(health <= 0)
            {
                if (lastTypeHitBy.Equals("fire"))
                {
                    glassDeath = true;
                    this.gameObject.tag = "Friendly";
                }
                else
                {
                    death = true;
                    Destroy(gameObject, deathAnimationLength);
                }
                stomp1 = false;
                stomp2 = false;
                finishStomp1 = false;
                finishStomp2 = false;
                rb.velocity = new Vector2(0, 0);
                Destroy(healthBar);
            }
		}
	}
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "Player" && !death && !glassDeath && !glassForm)
        {
            player.BroadcastMessage("applyDamage", damage);
            playerBox = c;
        }
        else if (c.gameObject.name.Equals("Player") && (glassForm || glassDeath))
        {
            takeGlass = true;
            glassForm = false;
            glassDeath = false;
            Destroy(gameObject, glassDeathAnimationLength);
            //PUT CODE FOR GIVING PLAYER INVENTORY STUFF HERE
        }
    }
}
