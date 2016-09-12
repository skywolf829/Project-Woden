using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const int LEFT = 0;
    private const int RIGHT = 1;
    private const int UP = 2;
    private const int DOWN = 3;
    private const int A = 4;
    private const int S = 5;
    private const int D = 6;
    private const int F = 7;
    private const int LSHIFT = 8;

    private const int NUMBEROFBUTTONS = 9;
    private bool[] inputArray;

    //different max speeds and scalars
    private float jumpMaxSpeed = 7.5f;
    private float moveSpeed = 1.0f;
    private float maxMoveSpeed = 7.0f;
    private float maxFallSpeed = 10.0f;

    //player boolean variables
    private bool falling;
	private bool jumping;
	private bool shooting;
	private bool wallShooting;
	private bool wallGrabbing;
	private bool crouching;
	private bool facingRight;
	private bool upPressed;
    private bool downPressed;
    private bool justHit;
    private bool justJump;
    private bool healed;
    private bool channeling;
    private bool tintChanged;

    private bool wallGrabJumpReset;
	private bool wallGrabLeft;
	private bool wallGrabRight;
    private bool nextToLeftWall;
    private bool nextToRightWall;
    private bool wallGrabJumpGrace;
    private bool wallGrabResetting;

    private float x;
    private float y;
    private float spawnx, spawny;
    private float dx;
    private float dy;
    private float horizontal;
    private float vertical;
    private float saveX;
    private float saveTopY;
    private float saveBotY;
    private float timeOfHit;
    private float colorOnInterval;
    private float colorOffInterval;
    private float colorOnIntervalSave;
    private float colorOffIntervalSave;
    private float interval;

	//animation times
	private float shootTime;
	private float finishShot;

    private float wallGrabJumpTime;
    private float wallGrabJumpLength;

    private float wallGrabResetTime;
    private float wallGrabResetLength;

    private float jumpStart;
    private float extendJumpTime;

    private float healTime;
    private float healDuration;

    private float emberStormDuration;
    private float emberStormTime;

	//player ints
	private int health;
	private int maxHealth;
	private int mana;
	private int maxMana;

	private int lightBlastCost;
	private int fireBlastCost;
	private int frostBlastCost;
	private int waterBlastCost;
	private int fateJavelineCost;
	private int emberStormCost;
	private int prayerCost;

    private Vector4 colorOn = new Vector4(1, 1, 1, 1);
    private Vector4 colorOff = new Vector4(1, 1, 1, 0);

	//prefabs for the projectiles
	public GameObject LightBlast;
    public GameObject FireBlast;
    public GameObject FrostBlast;
    public GameObject WaterBlast;
    public GameObject FateJaveline;
    public GameObject EmberStorm;
    public GameObject Prayer;
    private GameObject emberStormInstance;

    public Font HUDfont;

	//collision spots
	private Vector2 topLeft;
	private Vector2 topRight;
	private Vector2 botLeft;
	private Vector2 botRight;

    //obtaining the body of the player
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Transform tran;

    //obtaining the animation of the player
    private Animator anim;
    private SpriteRenderer sprites;


	private void Awake(){
		Application.targetFrameRate = 120;
	}
    //initializing the player
    private void Start()
    {
        //obtain player Unity traits
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = gameObject.GetComponent<BoxCollider2D>();
        tran = gameObject.GetComponent<Transform>();
        sprites = gameObject.GetComponent<SpriteRenderer>();

        //set max health and mana
        health = maxHealth = 100;
		mana = maxMana = 100;

        //initialize costs
		lightBlastCost = 0;
		fireBlastCost = 0;
		frostBlastCost = 0;
		waterBlastCost = 0;
		fateJavelineCost = 25;
		emberStormCost = 100;
		prayerCost = 70;

        colorOnInterval = colorOnIntervalSave = 0.25f;
        colorOffInterval = colorOffIntervalSave = 0.0f;
        interval = 0.5f;
        healDuration = 30f;
        emberStormDuration = 3.0f;

        extendJumpTime = 0.25f;

        //the length of the shooting animation
        shootTime = 0.5f;
        wallGrabJumpLength = 0.1f;
        wallGrabResetLength = 0.12f;
        //start facing right
        facingRight = true;
        wallGrabbing = false;

        inputArray = new bool[NUMBEROFBUTTONS];

        spawnx = tran.position.x;
        spawny = tran.position.y;
    }

    //called on a fixed interval
    private void Update()
    {
        checkWallProximity();

        updateWallgrab();
        updateInAir();

        handleMovementInput();
        handleMovement();
        handleButtonRefresh();
        handleShooting();   	

        updateAnimation();

        updateCollider();  
		updateGravity ();
        updateHeal();
        updateEmberStorm();
    }

    private void handleShooting()
    {
        if (!shooting && !channeling)
        {

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
            {
                GameObject projectile = GameObject.Instantiate<GameObject>(FateJaveline);
                Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
                if (projectile != null)
                {
                    if (facingRight)
                    {
                        projectile.transform.position = new Vector2(transform.position.x + 0.75f, transform.position.y);
                        projectileBody.BroadcastMessage("isFacingRight", facingRight);
                    }
                    else
                    {
                        projectile.transform.position = new Vector2(transform.position.x - 0.75f, transform.position.y);
                        projectileBody.BroadcastMessage("isFacingRight", facingRight);
                    }
                    mana -= fateJavelineCost;
                    shooting = true;
                    crouching = false;
                    finishShot = Time.time + shootTime;
					projectile.transform.tag = "PlayerProjectile";
                }                
            }
            else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
            {
                GameObject projectile = GameObject.Instantiate<GameObject>(Prayer);
                if (projectile != null)
                {
                    projectile.transform.position = new Vector2(transform.position.x, transform.position.y);
                    prayer();
                    mana -= prayerCost;
                    shooting = true;
                    crouching = false;
                    finishShot = Time.time + shootTime;
					projectile.transform.tag = "PlayerProjectile";

                }                
            }
            else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
            {
                GameObject projectile = GameObject.Instantiate<GameObject>(EmberStorm);
                emberStormInstance = projectile;
                if (emberStormInstance != null)
                {
                    channeling = true;
                    mana -= emberStormCost;
                    channeling = true;
                    crouching = false;
                    finishShot = Time.time + emberStormDuration;
					projectile.transform.tag = "PlayerProjectile";

                }                
            }
            else if (Input.GetKey(KeyCode.A))
            {
                GameObject projectile = GameObject.Instantiate<GameObject>(FrostBlast);
                Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
                if (projectile != null)
                {
                    if (facingRight)
                    {
                        projectile.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(5.5f, 0);
                    }
                    else
                    {
                        projectile.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(-5.5f, 0);
                    }
                    shooting = true;
                    crouching = false;
                    finishShot = Time.time + shootTime;
					projectile.transform.tag = "PlayerProjectile";

                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                GameObject projectile = GameObject.Instantiate<GameObject>(LightBlast);
                Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
                if (projectile != null)
                {
                    if (facingRight)
                    {
                        projectile.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(8f, 0);
                    }
                    else
                    {
                        projectile.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(-8f, 0);
                    }
                    shooting = true;
                    crouching = false;
                    finishShot = Time.time + shootTime;
					projectile.transform.tag = "PlayerProjectile";

                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                GameObject projectile = GameObject.Instantiate<GameObject>(WaterBlast);
                Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
                if (projectile != null)
                {
                    if (facingRight)
                    {
                        projectile.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(6f, 0);
                    }
                    else
                    {
                        projectile.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(-6f, 0);
                    }
                    shooting = true;
                    crouching = false;
                    finishShot = Time.time + shootTime;
					projectile.transform.tag = "PlayerProjectile";

                }
            }
            else if (Input.GetKey(KeyCode.F))
            {
                GameObject projectile = GameObject.Instantiate<GameObject>(FireBlast);
                Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
                if (projectile != null)
                {
                    if (facingRight)
                    {
                        projectile.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(7f, 0);
                    }
                    else
                    {
                        projectile.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
                        projectileBody.velocity = new Vector2(-7f, 0);
                    }
                    shooting = true;
                    crouching = false;
                    finishShot = Time.time + shootTime;
					projectile.transform.tag = "PlayerProjectile";

                }
            }
        }
    }
    private void handleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) || !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 0;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1;
        }


        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow) || !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            vertical = 0;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            vertical = -1;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            vertical = 1;
        }

        
    }
    private void handleMovement()
    {
        x = tran.position.x;
        y = tran.position.y;

        dx = rb.velocity.x;
        dy = rb.velocity.y;


        //arrow keys on the ground or in air
        if (horizontal != 0)
        {
            if (!wallGrabbing && !crouching)
            {
                if (horizontal < 0 && !nextToLeftWall)
                {
                    if (dx < 0)
                    {
                        dx -= moveSpeed;
                    }
                    else
                    {
                        dx -= moveSpeed * 1.5f;
                    }
                }
                else if (horizontal > 0 && !nextToRightWall)
                {
                    if (dx < 0)
                    {
                        dx += moveSpeed * 1.5f;
                    }
                    else
                    {
                        dx += moveSpeed;
                    }
                }

                else if (!wallGrabResetting && horizontal < 0 && nextToLeftWall && (falling || jumping))
                {
                    wallGrabbing = true;
                    wallGrabLeft = true;
                    dx = 0;
                    dy = 0;
                    x = saveX;
                    jumping = false;
                    if (!facingRight)
                    {
                        Flip();
                    }
                }
                else if (!wallGrabResetting && horizontal > 0 && nextToRightWall && (falling || jumping))
                {
                    wallGrabbing = true;
                    wallGrabRight = true;
                    dx = 0;
                    dy = 0;
                    x = saveX;
                    jumping = false;
                    if (facingRight)
                    {
                        Flip();
                    }
                }
            }
            if (wallGrabbing)
            {
                if (wallGrabLeft && horizontal > 0)
                {
                    x += 0.15f;
                    wallGrabLeft = false;
                    wallGrabbing = false;
                    falling = true;
                    wallGrabResetTime = Time.time;
                    wallGrabResetting = true;
                    wallGrabJumpGrace = true;
                    wallGrabJumpTime = Time.time;
                }
                else if (wallGrabRight && horizontal < 0)
                {
                    x -= 0.15f;
                    wallGrabRight = false;
                    wallGrabbing = false;
                    falling = true;
                    wallGrabResetTime = Time.time;
                    wallGrabResetting = true;
                    wallGrabJumpGrace = true;
                    wallGrabJumpTime = Time.time;
                }
            }
        }
        else if (horizontal == 0 && dx != 0)
        {
            if (dx > 0)
            {
                dx -= moveSpeed * 1.5f;
                if (dx < 0)
                {
                    dx = 0;
                }
            }
            else if (dx < 0)
            {
                dx += moveSpeed * 1.5f;
                if (dx < 0)
                {
                    dx = 0;
                }
            }
        }
        //up or down is pressed
        if (vertical != 0)
        {
            //down is pressed
            if (vertical < 0)
            {
                //crouch if they aren't falling or jumping
                if (!falling && !jumping && !wallGrabbing && !wallGrabResetting)
                {
                    crouching = true;
                    dy = 0;
                    dx = 0;
                }
                else if (wallGrabbing && !downPressed)
                {
                    wallGrabbing = false;
                    if (wallGrabLeft)
                    {
                        x += 0.15f;
                        wallGrabLeft = false;
                        falling = true;
                        wallGrabResetTime = Time.time;
                        wallGrabResetting = true;
                    }
                    else if (wallGrabRight)
                    {
                        x -= 0.15f;
                        wallGrabRight = false;
                        falling = true;
                        wallGrabResetTime = Time.time;
                        wallGrabResetting = true;
                    }
                }

            }
            //up is pressed
            else if (vertical > 0)
            {
                crouching = false;
                //can only jump if not jumping or falling
                if (!jumping && !falling && !wallGrabbing)
                {
                    dy = jumpMaxSpeed;
                    jumping = true;
                    jumpStart = Time.time;
                    justJump = true;
                }
                else if (wallGrabbing && !upPressed)
                {
                    jumpStart = Time.time;
                    justJump = true;
                    wallGrabbing = false;
                    if (wallGrabLeft)
                    {
                        x += 0.15f;
                        dy = jumpMaxSpeed;
                        wallGrabLeft = false;
                        jumping = true;
                        wallGrabResetTime = Time.time;
                        wallGrabResetting = true;
                    }
                    else if (wallGrabRight)
                    {
                        x -= 0.15f;
                        dy = jumpMaxSpeed;
                        wallGrabRight = false;
                        jumping = true;
                        wallGrabResetTime = Time.time;
                        wallGrabResetting = true;
                    }
                }
                else if (falling && wallGrabJumpGrace && !upPressed)
                {
                    dy = jumpMaxSpeed;
                    jumping = true;
                    jumpStart = Time.time;
                    justJump = true;
                }

            }
            if (jumping)
            {

                if (Time.time > (jumpStart + extendJumpTime))
                {
                    justJump = false;
                }
                if (justJump && !falling)
                {
                    dy = jumpMaxSpeed;
                }
            }
        }
        //final checks for the speed limits
        else if (vertical == 0)
        {
            crouching = false;
        }
        if (dy < -maxFallSpeed)
        {
            dy = -maxFallSpeed;
        }
        if (dx < -maxMoveSpeed)
        {
            dx = -maxMoveSpeed;
        }
        else if (dx > maxMoveSpeed)
        {
            dx = maxMoveSpeed;
        }
        //update the position and velocity
        tran.position = new Vector2(x, y);
        rb.velocity = new Vector2(dx, dy);

    }
    private void handleButtonRefresh()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            upPressed = true;
        }
        else
        {
            upPressed = false;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            downPressed = true;
        }
        else
        {
            downPressed = false;
        }
    }
    private void checkWallProximity()
    {
        if (nextToLeftWall || nextToRightWall)
        {
            if (tran.position.y + box.size.y / 2.0f < saveBotY)
            {
                wallGrabbing = false;
                nextToLeftWall = false;
                nextToRightWall = false;
            }
            else if (tran.position.y - box.size.y / 2.0f > saveTopY)
            {
                wallGrabbing = false;
                nextToLeftWall = false;
                nextToRightWall = false;
            }
        }
        if (nextToLeftWall)
        {
            
            if(tran.position.x > saveX + 0.2f)
            {
                nextToLeftWall = false;
            }
        }
        else if (nextToRightWall)
        {
            
            if (tran.position.x < saveX - 0.2f)
            {
                nextToRightWall = false;
            }
        }
    }

    public void hitBy(string t)
    {

    }

    private void prayer()
    {
        health += 80;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healed = true;
        tintChanged = false;
        healTime = Time.time;
    }  
    private void updateHeal()
    {
        if (healed)
        {
            if (!tintChanged)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                tintChanged = true;
            }
            if (Time.time > healTime + healDuration)
            {
                healed = false;
                tintChanged = false;
            }
        }
        else
        {
            if (!tintChanged)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                tintChanged = true;
            }
        }
    }    
    private void updateEmberStorm()
    {
        if (channeling && emberStormInstance != null)
        {
            if (facingRight)
            {
                emberStormInstance.transform.position = new Vector2(tran.position.x + 0.95f, tran.position.y + 0.1f);
            }
            else
            {
                emberStormInstance.transform.position = new Vector2(tran.position.x - 0.95f, tran.position.y + 0.1f);
            }            
        }
        if (Time.time > finishShot)
        {
            channeling = false;
            Destroy(emberStormInstance);
            emberStormInstance = null;
        }
    }

    private void updateInAir()
    {
        if(rb.velocity.y < 0 && !wallGrabbing)
        {
            jumping = false;
            falling = true;
        }
        
        if (falling)
        {
            if(rb.velocity.y == 0)
            {
                falling = false;
            }
            if (wallGrabbing)
            {
                falling = false;
            }
        }
    }

    private void updateAnimation()
    {
        //flip the player to face the right direction
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !facingRight && !wallGrabbing)
        {
            Flip();
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && facingRight && !wallGrabbing)
        {
            Flip();
        }

		if (shooting && Time.time > finishShot) {
			shooting = false;
		}        
        if (shooting && !wallGrabbing)
        {
            anim.Play("PlayerShoot");
        }
        else if(shooting && wallGrabbing)
        {
            anim.Play("PlayerWallShoot");
        }

        //update animation
		if (!wallGrabbing) {
			if(Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetFloat("speed", 1.0f);
            }
            else if(Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetFloat("speed", 1.0f);
            }
            else
            {
                anim.SetFloat("speed", 0);
            }

		} else {
			anim.SetFloat ("speed", 0f);
		}

        if (justHit)
        {
            if(Time.time > 1.48f + timeOfHit)
            {
                justHit = false;
                colorOffInterval = colorOffIntervalSave;
                colorOnInterval = colorOnIntervalSave;
            }
            else if(Time.time > timeOfHit + colorOffInterval)
            {
                colorOffInterval += interval;
                sprites.color = colorOff;
            }
            else if(Time.time > timeOfHit + colorOnInterval)
            {
                colorOnInterval += interval;
                sprites.color = colorOn;
            }
        }

        anim.SetBool("falling", falling);
        anim.SetBool("wallGrabbing", wallGrabbing);
        anim.SetBool("jumping", jumping);
        anim.SetBool("crouching", crouching);
        anim.SetBool("channeling", channeling);
    }

	void updateWallgrab(){
        if(wallGrabResetting && Time.time > wallGrabResetTime + wallGrabResetLength)
        {
            wallGrabResetting = false;
        }
        if(wallGrabJumpGrace && Time.time > wallGrabJumpLength + wallGrabJumpTime)
        {
            wallGrabJumpGrace = false;
        }
    }

	void OnTriggerEnter2D(Collider2D c){
		//handle getting hit by projectiles
	}

	public void applyDamage(int d){
        if (!justHit && d != 0)
        {
            health -= d;
            justHit = true;
            timeOfHit = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {

        if (c.gameObject.tag.Equals("Boundary"))
        {
            tran.position = new Vector2(spawnx, spawny);
        }
        //handdle wall grabbing
        else if(c.gameObject.name == "RectangleObject")
        {
            Vector2 mapCollisionPos = c.gameObject.GetComponent<Transform>().position;
            Vector2 mapCollisionSize = c.gameObject.GetComponent<BoxCollider2D>().size;

			topLeft = new Vector2(mapCollisionPos.x, mapCollisionPos.y);
			topRight = new Vector2(mapCollisionPos.x + mapCollisionSize.x, mapCollisionPos.y);
			botLeft = new Vector2(mapCollisionPos.x, mapCollisionPos.y - mapCollisionSize.y);
			botRight = new Vector2(mapCollisionPos.x + mapCollisionSize.x, mapCollisionPos.y - mapCollisionSize.y);            

			if(topLeft.y <= tran.position.y && tran.position.x > topLeft.x && tran.position.x < topRight.x){
                //Debug.Log ("bottom");
                if (wallGrabbing)
                {
                    wallGrabbing = false;
                    if (wallGrabLeft)
                    {
                        tran.position = new Vector2(tran.position.x + 0.15f, tran.position.y);
                        wallGrabLeft = false;
                    }
                    else if (wallGrabRight)
                    {
                        tran.position = new Vector2(tran.position.x - 0.15f, tran.position.y);
                        wallGrabRight = false;
                    }
                }
            }
			else if(botLeft.y >= tran.position.y && tran.position.x > botLeft.x && tran.position.x < botRight.x){
                //Debug.Log ("top");
                rb.velocity = new Vector2(rb.velocity.x, -0.01f);
                falling = true;
			}
			else if(topRight.x <= tran.position.x){
                //Debug.Log("left");
				saveX = topRight.x + (box.size.x / 2.0f);
                nextToLeftWall = true;
                saveBotY = botLeft.y;
                saveTopY = topLeft.y;
            }
			else if(topLeft.x > tran.position.x){
                //Debug.Log("right");
				saveX = topLeft.x - (box.size.x / 2.0f);
                saveBotY = botLeft.y;
                saveTopY = topLeft.y;				
                nextToRightWall = true;
                
            }
            else
            {
                //not really sure. Weird.
            }
        }
    }

    private void updateCollider()
    {
        if (crouching)
        {
            box.size = new Vector2(0.75f, 0.65f);
            box.offset = new Vector2(-0.03f, -0.17f);
        }
        else
        {
            box.size = new Vector2(0.75f, 0.96f);
            box.offset = new Vector2(0.056f, -0.02f);            
        }
    }

	private void updateGravity(){
		if (wallGrabbing) {
			rb.gravityScale = 0.085f;
		}
        else {
            if (jumping && vertical > 0 && justJump)
            {
                rb.gravityScale = 0f;
            }
            else
            {
                rb.gravityScale = 3.53f;
            }                   
		}
	}

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.font = HUDfont;
        myStyle.fontSize = 20;
        GUI.Label(new Rect(10, 10, 200, 20), health + " / " + maxHealth, myStyle);
        GUI.Label(new Rect(10, 30, 200, 20), mana + " / " + maxMana, myStyle);
    }

    private void Flip()
    {
        //Switch the way the player is facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
}