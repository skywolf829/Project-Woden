using UnityEngine;
using System.Collections;

public class SandSpike : MonoBehaviour {

    private bool started;

    private float animationLength;
    private float contractTime;
    private float startTime;
    private bool controlled;

    private int damage;

    Animator anim;
    Rigidbody2D rb;
    
    Transform tran;
	GameObject player;
    GameObject healthBar;
    Collider2D playerBox;
    
    public BoxCollider2D damageBox;

	// Use this for initialization
	void Start () {
        damage = 20;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tran = gameObject.GetComponent<Transform>();
		player = GameObject.Find ("Player");
        playerBox = new Collider2D();
        anim.SetBool("contracted", true);
        started = false;
        animationLength = 1.04f;
        startTime = Time.time;
        
    }
	public void timeContracted(float t)
    {
        contractTime = t;
        controlled = true;
    }
	// Update is called once per frame
	void Update () {
        AI();
        checkHit();
	}
    

    private void checkHit()
    {
        if (damageBox.IsTouching(playerBox))
        {
            player.BroadcastMessage("applyDamage", damage);
        }
    }

    public void hitBy(string s)
    {

    }

    public void applyDamage(int d)
    {

    }

    private void AI()
    {
        if (controlled && Time.time > startTime + contractTime)
        {
            anim.SetBool("contracted", false);
            Destroy(gameObject, animationLength);
            controlled = false;
            started = true;
        }
        else if (!controlled && !started)
        {
            started = true;
            anim.SetBool("contracted", false);
            Destroy(gameObject, animationLength);
        }
    }
    void OnTriggerEnter2D(Collider2D c)
    {        
        if (c.gameObject.name == "Player")
        {
            player.BroadcastMessage("applyDamage", damage);
            playerBox = c;
        }      
    }
}
