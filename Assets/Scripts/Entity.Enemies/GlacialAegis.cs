using UnityEngine;
using System.Collections;

public class GlacialAegis : Enemy {
    private bool creation;
    private bool adjustedSize;

    private float creationTime;
    private float creationLength;
    private float healthBarPositionY;

    // Use this for initialization
    protected override void Start() {
        deathAnimationLength = 1.0f;

        health = maxHealth = 1000;

        aggro = false;
        facingRight = true;
        creationTime = Time.time;

        lastTypeHitBy = "";

        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tran = gameObject.GetComponent<Transform>();
        player = GameObject.Find("Player");
        healthBar = GameObject.Instantiate<GameObject>(HealthBar);
        healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2.0f), tran.position.y + healthBarPositionY);

        strength = "ice";
        weakness = "fire";
    }

    // Update is called once per frame
    protected override void Update() {
        AI();
        updateAnimation();
        updateHealthBar();
    }
    protected override void AI()
    {        
        dx = rb.velocity.x;
        dy = rb.velocity.y;
        x = tran.position.x;
        y = tran.position.y;

        if (!death)
        {
            if (!adjustedSize)
            {
                tran.localScale = new Vector3(damageBox.size.x + 0.75f, damageBox.size.y + 0.5f, 1);
                damageBox.size = new Vector2(damageBox.size.x / (damageBox.size.x + 0.75f), damageBox.size.y / (damageBox.size.y + 0.5f));
                adjustedSize = true;
            }
            y += 0.1f;
        }            
        tran.position = new Vector2(x, y);        
    }
    protected override void updateAnimation()
    {
        if (Time.time > creationTime + creationLength && creation) {
            creation = false;
        }
        anim.SetBool("death", death);
    }
    public void setHitBox(BoxCollider2D d)
    {
        damageBox.size = new Vector2(d.size.x, d.size.y);
        healthBarPositionY = -0.105f + damageBox.size.y / 2.0f + 0.154f;
    }
    protected override void updateHealthBar()
    {
        if (!death)
        {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2.0f), tran.position.y + healthBarPositionY);
            healthBar.transform.localScale = new Vector3((float)health / maxHealth + 0.1f, 1.2f, 1);
        }        
    }

	public override void applyDamage(int d){
        base.applyDamage(d);
        if (!death) {
            if (health <= 0)
            {
                death = true;
                Destroy(healthBar);
                Destroy(damageBox);
                Destroy(gameObject, deathAnimationLength);                              
            }
            if(health > maxHealth)
            {
                health = maxHealth;
            }
		}
	}
    protected override void OnTriggerEnter2D(Collider2D c) { }
    protected override void OnCollisionEnter2D(Collision2D c) { }
}
