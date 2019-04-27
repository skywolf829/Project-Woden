using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyV2 : MonoBehaviour
{
    protected string lastTypeHitBy;
    protected string weakness;
    protected string strength;

    protected bool death;
    protected bool facingRight;
       
    protected int health;
    protected int maxHealth;
    protected int collisionDamage;

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

    protected virtual void Start()
    {
        facingRight = true;

        anim = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        tran = gameObject.GetComponent<Transform>();

        if (HealthBar)
        {
            healthBar = GameObject.Instantiate<GameObject>(HealthBar);
        }
        else print("Missing healthbar on " + gameObject.name);

        player = GameObject.FindWithTag("Player");
        playerBox = new Collider2D();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bosses = GameObject.FindGameObjectsWithTag("Boss");

        if (gameObject.tag == "Enemy" || gameObject.tag == "Friendly")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), collisionBox);
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            Physics2D.IgnoreCollision(enemies[i].GetComponent<Collider2D>(), collisionBox);
        }
        for (int i = 0; i < bosses.Length; i++)
        {
            Physics2D.IgnoreCollision(bosses[i].GetComponent<Collider2D>(), collisionBox);
        }
        StartCoroutine(AI());
    }
    protected abstract IEnumerator AI();
    protected abstract void TriggerDeathStart();
    protected virtual void updateHealthBar()
    {
        if (!death && healthBar)
        {
            healthBar.transform.position = new Vector2(tran.position.x - (((1 - (float)health / maxHealth)) / 2), tran.position.y + collisionBox.size.y / 2.0f + 0.15f);
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1f, 1);
        }
    }
    public void hitBy(string t)
    {
        lastTypeHitBy = t;
    }
    public virtual void applyDamage(int d)
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
    protected virtual void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "Player")
        {
            player.BroadcastMessage("applyDamage", collisionDamage);
            playerBox = c;
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Boundary"))
        {
            death = true;            
            Destroy(healthBar);
            TriggerDeathStart();
        }
        else if (c.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(c.collider, collisionBox);
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
