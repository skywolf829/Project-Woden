using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marman : EnemyV2
{
    bool inNonIdleState = false, inIdleState = false;
    bool idleAnim, charging, walking, stepSlashing, hitStun, spinning;
    float moveSpeed;
    bool aggroed = false;
    string lastAttackType;       
    Vector3 savedPlayerPos;
    bool lastHitOnRight = false, lastHitOnLeft = false;
    bool breakCharge = false;
    bool finishedStepSlash = false;
    bool finishedCharge = false;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 1;
        health = maxHealth = 400;
        collisionDamage = 0;
        weakness = "frost";
        strength = "water";
    }
    public override void applyDamage(int d)
    {
        base.applyDamage(d);        
        if (!death && health <= 0)
        {
            health = 0;
            death = true;
            TriggerDeathStart();
        }
        else
        {
            HitStun();
        }
    }
    void HitStun()
    {
        hitStun = true;
        StopAllCoroutines();
        anim.ResetTrigger("StartCharge");
        anim.ResetTrigger("EndCharge");
        anim.ResetTrigger("Idle1");
        anim.ResetTrigger("Idle2");
        anim.ResetTrigger("StepSlash");
        anim.SetBool("walking", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
        inNonIdleState = false;
        inIdleState = false;
        idleAnim = charging = walking = stepSlashing = false;
        anim.SetTrigger("HitStun");
        StartCoroutine(AI());
    }
    protected override IEnumerator AI()
    {
        float distToPlayer;
        while (true)
        {
            updateHealthBar();
            distToPlayer = Vector2.Distance(transform.GetChild(0).position, player.transform.position);
            if (!aggroed && distToPlayer < 5)
            {
                aggroed = true;
            }
            else if(aggroed && distToPlayer > 12)
            {
                aggroed = false;
            }
            if (aggroed && !inNonIdleState && !death && !hitStun)
            {
                float rand = Random.Range(0f, 1f);
                if(distToPlayer < 2)
                {
                    if(rand < 0.6f)
                    {
                        StartCoroutine(StepSlashState());
                    }
                    else
                    {
                        StartCoroutine(StepSlashTwiceState());
                    }
                }   
                else if(distToPlayer < 7)
                {
                    if(rand < 0.2f)
                    {
                        StartCoroutine(ChargeState());
                    }
                    else if(rand < 0.6f)
                    {
                        StartCoroutine(MoveTowardPlayer1SecondState());
                    }
                    else
                    {
                        StartCoroutine(MoveTowardPlayerUntil2UnitsState());
                    }
                }
                else
                {
                    if (rand < 0.25f)
                    {
                        StartCoroutine(Idle1State());
                    }
                    else if (rand < 0.55f)
                    {
                        StartCoroutine(MoveTowardPlayer2SecondsState());
                    }
                    else if (rand < 0.85f)
                    {
                        StartCoroutine(MoveTowardPlayerUntil2UnitsState());
                    }
                    else
                    {
                        StartCoroutine(ChargeState());
                    }
                }
            }
            else if (!aggroed && !inIdleState && !hitStun)
            {
                float x = Random.Range(0f, 1f);
                if(x < 0.3f)
                {
                    StartCoroutine(Idle2State());
                }
                else
                {
                    StartCoroutine(Idle1State());
                }
            }
            yield return null;
        }
    }
    IEnumerator Idle1State()
    {
        inIdleState = true;
        anim.SetTrigger("Idle1");
        yield return new WaitForSeconds(3f);
        inIdleState = false;
        yield return null;
    }
    IEnumerator Idle2State()
    {
        inIdleState = true;
        anim.SetTrigger("Idle2");
        yield return new WaitForSeconds(3f);
        inIdleState = false;
        yield return null;
        yield return null;
    }
    IEnumerator MoveTowardPlayer2SecondsState()
    {
        inNonIdleState = true;
        walking = true;
        anim.SetBool("walking", true);
        float startTime = Time.time;
        while(Time.time < startTime + 2) { 
            int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
            transform.localScale = new Vector3(dir, 1, 1);
            rb.velocity = new Vector2(dir, rb.velocity.y);
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        walking = false;
        anim.SetBool("walking", false);
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator MoveTowardPlayerUntil2UnitsState()
    {
        inNonIdleState = true;
        walking = true;
        anim.SetBool("walking", true);
        float startTime = Time.time;
        while (Vector2.Distance(transform.GetChild(0).position, player.transform.position) > 2)
        {
            int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
            transform.localScale = new Vector3(dir, 1, 1);
            rb.velocity = new Vector2(dir, rb.velocity.y);
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        walking = false;
        anim.SetBool("walking", false);
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator ChargeState()
    {
        inNonIdleState = true;
        anim.SetTrigger("StartCharge");
        transform.GetChild(1).gameObject.SetActive(true);
        int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        while (!charging)
        {
            yield return null;
        }
        rb.velocity = new Vector2(dir * 3, rb.velocity.y);
        bool canCharge = true;
        float timeOfPass = 0;
        bool passedPlayer = false;
        while (canCharge) {

            rb.velocity = new Vector2(dir * 3, rb.velocity.y);
            if (!passedPlayer && (
                dir != (player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1) || breakCharge))
            {
                passedPlayer = true;
                timeOfPass = Time.time;
            }
            if(passedPlayer && Time.time > timeOfPass + 1.5f)
            {
                canCharge = false;
            }
            if (Mathf.Abs(rb.velocity.x) < 1) canCharge = false;
            yield return null;
        }
        anim.SetTrigger("EndCharge");
        rb.velocity = new Vector2(0, rb.velocity.y);
        transform.GetChild(1).gameObject.SetActive(false);
        while (!finishedCharge)
        {
            yield return null;
        }
        finishedCharge = false;
        breakCharge = false;
        charging = false;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator MoveTowardPlayer1SecondState()
    {
        inNonIdleState = true;
        walking = true;
        anim.SetBool("walking", true);
        float startTime = Time.time;
        while (Time.time < startTime + 1)
        {
            int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
            transform.localScale = new Vector3(dir, 1, 1);
            rb.velocity = new Vector2(dir, rb.velocity.y);
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        walking = false;
        anim.SetBool("walking", false);
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator StepSlashState()
    {
        inNonIdleState = true;
        stepSlashing = true;
        anim.SetTrigger("StepSlash");
        int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        while (!finishedStepSlash)
        {
            yield return null;
        }
        finishedStepSlash = false;
        stepSlashing = false;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator StepSlashTwiceState()
    {
        inNonIdleState = true;
        stepSlashing = true;

        int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        anim.SetTrigger("StepSlash");
        while (!finishedStepSlash)
        {
            yield return null;
        }
        finishedStepSlash = false;

        dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        anim.SetTrigger("StepSlash");
        while (!finishedStepSlash)
        {
            yield return null;
        }
        finishedStepSlash = false;
        stepSlashing = false;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator SpinningState()
    {
        Destroy(healthBar);
        spinning = true;
        // Hit on left
        if (lastCollisionPosition.x < transform.GetChild(0).position.x)
        {
            // Facing right, spin forward
            if(transform.localScale.x > 0)
            {
                anim.SetTrigger("SpinForward");
            }
            else
            {
                anim.SetTrigger("SpinBackward");
            }
            rb.freezeRotation = false;
            rb.AddForce(new Vector2(150, 300));
            rb.AddForceAtPosition(new Vector2(100, 0), transform.GetChild(0).position + new Vector3(0, 0.5f, 0));
            rb.AddForceAtPosition(new Vector2(-100, 0), transform.GetChild(0).position - new Vector3(0, 0.5f, 0));

        }
        // Hit on right
        else
        {
            // Facing right, spin backward
            if(transform.localScale.x > 0)
            {
                anim.SetTrigger("SpinBackward");
            }
            else
            {
                anim.SetTrigger("SpinForward");
            }
            rb.freezeRotation = false;
            rb.AddForce(new Vector2(-150, 300));
            rb.AddForceAtPosition(new Vector2(-100, 0), transform.GetChild(0).position + new Vector3(0, 0.5f, 0));
            rb.AddForceAtPosition(new Vector2(100, 0), transform.GetChild(0).position - new Vector3(0, 0.5f, 0));
        }
        Vector2 lastV = new Vector2(1, 1);
        float startTime = Time.time;
        while(Time.time < startTime + 0.5f || Mathf.Abs(rb.velocity.magnitude) > 0.7f || Mathf.Abs((lastV - rb.velocity).magnitude) / Time.deltaTime > 0.7f)
        {
            lastV = rb.velocity;
            yield return null;
        }
        rb.velocity = Vector2.zero;
        spinning = false;
        anim.SetTrigger("Death");
        anim.SetBool("dead", true);
        Destroy(collisionBox);
        Destroy(rb);
        yield return null;
    }
    public void FinishHitStun()
    {
        hitStun = false;
    }
    public void StepSlashMovement()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        if(transform.localScale.x > 0)
        {
            transform.Translate(new Vector3(0.3f, 0, 0));
        }
        else
        {
            transform.Translate(new Vector3(-0.3f, 0, 0));
        }
    }
    public void FinishStepSlash()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        finishedStepSlash = true;
    }
    public void BeginCharge()
    {
        charging = true;
    }
    public void FinishCharge()
    {
        finishedCharge = true;
    }
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "Player")
        {
            if (spinning)
            {
                player.BroadcastMessage("applyDamage", 50);
            }
            else
            {
                player.BroadcastMessage("applyDamage", collisionDamage);
            }
            playerBox = c;
            if (charging)
            {
                breakCharge = true;
            }
        }
        if(c.gameObject.tag == "Enemy" && spinning)
        {
            c.gameObject.BroadcastMessage("hitBy", "physical");
            c.gameObject.BroadcastMessage("applyDamage", 50);
            rb.velocity = rb.velocity * -0.7f;
            print("Hit enemy trigger");
        }
    }
    protected override void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Boundary"))
        {
            death = true;
            Destroy(healthBar);
            TriggerDeathStart();
        }
        else if (c.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(c.collider, c.otherCollider);
        }
    }
    protected override void updateHealthBar()
    {
        if (!death && healthBar && maxHealth != 0)
        {
            Vector2 healthBarPos = transform.GetChild(3).position;
            healthBarPos += new Vector2(-(((1 - (float)health / maxHealth)) / 2), 0);
            Vector3 healthBarScale = new Vector3((float)health / maxHealth, 1f, 1);

            healthBar.transform.position = healthBarPos;
            healthBar.transform.localScale = healthBarScale;
        }
    }
    protected override void TriggerDeathStart()
    {
        StopAllCoroutines();
        anim.ResetTrigger("StartCharge");
        anim.ResetTrigger("EndCharge");
        anim.ResetTrigger("Idle1");
        anim.ResetTrigger("Idle2");
        anim.ResetTrigger("StepSlash");
        anim.SetBool("walking", false);
        StartCoroutine(SpinningState());
    }
    public void StartDestroy()
    {
        Destroy(gameObject);
    }
}
