using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    protected string lastTypeHitBy;
    protected string weakness;
    protected string strength;
    
    protected bool death;
    protected bool facingRight;
    protected bool left;
    protected bool right;
	protected bool aggro;
    protected bool shielded;

	protected float deathAnimationLength;
	protected float finishDeath;

    protected float moveSpeed;
    protected float maxSpeed;
    protected float stopSpeed;
    protected float maxFallSpeed;

    protected float aggroTime;

    protected int health;
    protected int maxHealth;
    protected int damage;
    protected int range;

    protected float x, y, dx, dy;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected GameObject healthBar;
    protected Transform tran;

	protected GameObject player;
    protected Collider2D playerBox;

    protected GameObject[] enemies;
    protected GameObject[] bosses;

    protected GameObject shield;

    public BoxCollider2D collisionBox;
    public BoxCollider2D damageBox;
	public GameObject HealthBar;

	protected virtual void Start() {
        aggro = false;
		facingRight = true;

        anim = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();  
        tran = gameObject.GetComponent<Transform>();

        if (HealthBar)
        {
            healthBar = GameObject.Instantiate<GameObject>(HealthBar);
        }

        player = GameObject.Find ("Player");
        playerBox = new Collider2D();		

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bosses = GameObject.FindGameObjectsWithTag("Boss");

		if (gameObject.tag == "Enemy" || gameObject.tag == "Friendly")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), collisionBox);
        }
        for(int i = 0; i < enemies.Length; i++)
        {
            Physics2D.IgnoreCollision(enemies[i].GetComponent<Collider2D>(), collisionBox);
        }
        for(int i = 0; i < bosses.Length; i++)
        {
            Physics2D.IgnoreCollision(bosses[i].GetComponent<Collider2D>(), collisionBox);
        }
    }
	
	// Update is called once per frame
	protected virtual void Update () {
		checkAggro ();
        AI();
        updateAnimation();
		updateHealthBar ();
        checkHit();
        checkShield();
    }

	protected void checkAggro(){
        if (player && Mathf.Abs(tran.position.x - player.transform.position.x) < range &&
            Mathf.Abs(tran.position.y - player.transform.position.y) < range / 2)
        {
            if(aggro == false)
            {
                aggroTime = Time.time;
            }
			aggro = true;
            
		} else {
			aggro = false;
		}
	}

    protected void checkHit()
    {
        if (damageBox.IsTouching(playerBox))
        {
            player.BroadcastMessage("applyDamage", damage);
        }
    }

    public void hitBy(string t)
    {
        lastTypeHitBy = t;
    }

    protected virtual void AI() { }

    protected IEnumerator createProjectile(GameObject proj, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject projectile = GameObject.Instantiate<GameObject>(proj);
        Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
        if (projectile != null)
        {
            projectile.transform.tag = "EnemyProjectile";
            if (facingRight)
            {
                projectile.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
            }
            else
            {
                projectile.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
            }
        }
    }

    protected IEnumerator createProjectile(GameObject proj, float speed, float time){
		yield return new WaitForSeconds(time);
		GameObject projectile = GameObject.Instantiate<GameObject>(proj);
		Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
		if(projectile != null){
			projectile.transform.tag = "EnemyProjectile";
			if (facingRight)
			{
				projectile.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
				projectileBody.velocity = new Vector2(speed, 0);
			}
			else
			{
				projectile.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
				projectileBody.velocity = new Vector2(-speed, 0);
			}
		}
	}
	protected IEnumerator createPlayerLockedProjectile(GameObject proj, float speed, float time){
		yield return new WaitForSeconds (time);
		GameObject projectile = Instantiate (proj);
		Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D> ();
		float playerx = player.transform.position.x;
		float playery = player.transform.position.y;
		float angleInDeg, projectileSpeedX, projectileSpeedY;
		angleInDeg = Mathf.Atan ((playery - tran.position.y) / (playerx - tran.position.x)) * Mathf.Rad2Deg;
		projectileSpeedX = speed * Mathf.Cos (angleInDeg * Mathf.Deg2Rad);
		projectileSpeedY = speed * Mathf.Sin (angleInDeg * Mathf.Deg2Rad);
		if (playerx < tran.position.x) {
			projectileSpeedY *= -1;
		}
	

		if(projectile != null){
			projectile.transform.tag = "EnemyProjectile";
			if (facingRight)
			{
				projectile.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeedX, projectileSpeedY);
				projectileBody.rotation = angleInDeg;
			}
			else
			{
				projectile.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
                projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeedX, projectileSpeedY);
                projectileBody.rotation = angleInDeg;
			}
		}
	}
    protected IEnumerator createProjectile(GameObject proj, float speed, float time, float x, float y)
    {
        yield return new WaitForSeconds(time);
        GameObject projectile = GameObject.Instantiate<GameObject>(proj);
        Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
        if (projectile != null)
        {
			projectile.transform.tag = "EnemyProjectile";

            if (facingRight)
            {
                projectile.transform.position = new Vector2(x, y);
                projectileBody.velocity = new Vector2(speed, 0);
            }
            else
            {
                projectile.transform.position = new Vector2(x, y);
                projectileBody.velocity = new Vector2(-speed, 0);
            }
        }
        else
        {
            Debug.Log("null");
        }
    }
	protected IEnumerator createRandomProjectile(GameObject proj, float speed, float time, float x, float y, float angle){
		yield return new WaitForSeconds(time);
		GameObject projectile = GameObject.Instantiate<GameObject>(proj);
		Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();

		float rand = Random.Range (-angle / 2.0f, angle / 2.0f);
		float projectileSpeedX = speed * Mathf.Cos (rand * Mathf.Deg2Rad);
		float projectileSpeedY = speed * Mathf.Sin (rand * Mathf.Deg2Rad);

		if (projectile != null)
		{
			projectile.transform.tag = "EnemyProjectile";
			projectile.SendMessage ("fixedVelocity");
			if (facingRight)
			{
				projectile.transform.position = new Vector2(x, y);
				projectileBody.velocity = new Vector2(projectileSpeedX, projectileSpeedY);
				projectileBody.rotation = rand;
			}
			else
			{
				projectile.transform.position = new Vector2(x, y);
				projectileBody.velocity = new Vector2(-projectileSpeedX, projectileSpeedY);
				projectileBody.rotation = rand;
			}
		}
		else
		{
			Debug.Log("null");
		}
	}
	protected IEnumerator createAngledProjectile(GameObject proj, float speed, float time, float x, float y, float angle){
		yield return new WaitForSeconds(time);
		GameObject projectile = GameObject.Instantiate<GameObject>(proj);
		Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();

		float projectileSpeedX = speed * Mathf.Cos (angle * Mathf.Deg2Rad);
		float projectileSpeedY = speed * Mathf.Sin (angle * Mathf.Deg2Rad);

		if (projectile != null)
		{
			projectile.transform.tag = "EnemyProjectile";
			projectile.SendMessage ("fixedVelocity");
			if (facingRight)
			{
				projectile.transform.position = new Vector2(x, y);
				projectileBody.velocity = new Vector2(projectileSpeedX, projectileSpeedY);
				projectileBody.rotation = angle;
			}
			else
			{
				projectile.transform.position = new Vector2(x, y);
				projectileBody.velocity = new Vector2(-projectileSpeedX, projectileSpeedY);
				projectileBody.rotation = -angle;
			}
		}
		else
		{
			Debug.Log("null");
		}
	}
	protected IEnumerator createPlayerLockedProjectile(GameObject proj, float speed, float time, float x, float y)
	{
		yield return new WaitForSeconds(time);
		GameObject projectile = GameObject.Instantiate<GameObject>(proj);
		Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();

		float playerx = player.transform.position.x;
		float playery = player.transform.position.y;
		float angleInDeg, projectileSpeedX, projectileSpeedY;
		angleInDeg = Mathf.Atan ((playery - y) / (playerx - x)) * Mathf.Rad2Deg;
		projectileSpeedX = speed * Mathf.Cos (angleInDeg * Mathf.Deg2Rad);
		projectileSpeedY = speed * Mathf.Sin (angleInDeg * Mathf.Deg2Rad);
		if (playerx < tran.position.x) {
			projectileSpeedY *= -1;
		}
		if (projectile != null)
		{
			projectile.transform.tag = "EnemyProjectile";

			if (facingRight)
			{
				projectile.transform.position = new Vector2(x, y);
				projectileBody.velocity = new Vector2(projectileSpeedX, projectileSpeedY);
				projectileBody.rotation = angleInDeg;
			}
			else
			{
				projectile.transform.position = new Vector2(x, y);
				projectileBody.velocity = new Vector2(-projectileSpeedX, projectileSpeedY);
				projectileBody.rotation = angleInDeg;
			}
		}
		else
		{
			Debug.Log("null");
		}
	}
    protected void createContractedProjectile(GameObject proj, float contractedTime, float x, float y)
    {
        GameObject projectile = GameObject.Instantiate<GameObject>(proj);
        if (projectile != null)
        {
			projectile.transform.tag = "Enemy";

            if (facingRight)
            {
                projectile.transform.position = new Vector2(x, y);
            }
            else
            {
                projectile.transform.position = new Vector2(x, y);
            }
            projectile.BroadcastMessage("timeContracted", contractedTime);
        }
    }
    protected virtual void updateAnimation() { }
	protected virtual void updateHealthBar()
    {
		if (!death && healthBar) 
        {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
        }		
	}
    
    public void giveShield(GameObject s)
    {
        if (shielded)
        {
            shield.BroadcastMessage("hitBy", "dead");
            shield.BroadcastMessage("applyDamage", 5000);
        }
        shielded = true;
        shield = s;
        if(shield != null)
        {
            shield.BroadcastMessage("setHitBox", damageBox);
        }
        
    }

	public GameObject scanForBattery(){
		GameObject b = null;
		GameObject[] allFriendlies = GameObject.FindGameObjectsWithTag ("Friendly");
		for (int i = 0; i < allFriendlies.Length; i++) {			
			if (allFriendlies [i].name.StartsWith ("Battery")) {
				if (!b) {
					b = allFriendlies [i];
				} else if (Vector2.Distance (allFriendlies [i].transform.position, transform.position) <
				        Vector2.Distance (b.transform.position, transform.position)) {
					b = allFriendlies [i];
				}
			}
		}

		return b;
	}
    public virtual void applyDamage(int d){
        if (shielded && shield != null)
        {
            shield.BroadcastMessage("applyDamage", d);
            
        }
        else
        {
            if (lastTypeHitBy.Equals(weakness))
            {
                health -= d * 4;
            }
            else if (lastTypeHitBy.Equals(strength))
            {
                health -= d / 4;
            }
            else if (lastTypeHitBy.Equals(""))
            {
                Debug.Log("No last hit type");
            }
            else
            {
                health -= d;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
            }            
        }
	}
    protected virtual void OnTriggerEnter2D(Collider2D c)
    {        
        if (c.gameObject.name == "Player")
        {
            player.BroadcastMessage("applyDamage", damage);
            playerBox = c;
        }        
        
    }
    protected virtual void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Boundary"))
        {
            death = true;
            if (shielded)
            {
                shield.BroadcastMessage("hitBy", "dead");
                shield.BroadcastMessage("applyDamage", 10000);
            }
            Destroy(healthBar);
            Destroy(this.gameObject, deathAnimationLength);
        }
        else if (c.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(c.collider, collisionBox);
        }
    }
    protected void checkShield()
    {
        if(shield)
        {
            if(shield.GetComponent<GlacialAegis>().health <= 0) {
                shield = null;
                shielded = false;
            }
            
        }
    }
	protected void Flip()
	{
		//Switch the way the player is facing
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
/*
	 * This is useful for boss battles!!!!!!
	void OnGUI(){
		//drawHealthBar ((int)(tran.position.x * 32), (int)(tran.position.y * 32 - 1), (int)(box.size.x * 32 * health / maxHealth),
		//               (int)(0.1f * 32), 255.0f, 1.0f, 1.0f);
		//drawHealthBar (0, 0, 15, 15, 255.0f, 0.0f, 0.0f);
	}
	void drawHealthBar(int x, int y, int w, int h, float r, float g, float b)
	{
		Texture2D rgb_texture = new Texture2D(w, h);
		Color rgb_color = new Color(r, g, b);
		int i, j;
		for(i = 0; i < w; i++)
		{
			for(j = 0; j < h; j++)
			{
				rgb_texture.SetPixel(i, j, rgb_color);
			}
		}
		rgb_texture.Apply();
		GUIStyle generic_style = new GUIStyle();
		GUI.skin.box = generic_style;
		GUI.Box (new Rect (x,y,w,h), rgb_texture);
	}
	*/
