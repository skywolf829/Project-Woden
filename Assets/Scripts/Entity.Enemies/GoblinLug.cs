using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinLug : EnemyV2
{

    List<Coroutine> currentCoroutines = new List<Coroutine>();
    bool inNonIdleState = false;
    bool idleAnim, step, walking, slashing, duckAttacking, stomping, doubleSlashing, slashDucking;
    float moveSpeed;
    float idleUntil = 0;

    string lastAttackType;



    Vector3 savedPlayerPos;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1;
        health = maxHealth = 3000;
        collisionDamage = 0;
        weakness = "death";
        strength = "dark";
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
    }

    protected override IEnumerator AI()
    {
        while (true)
        {
            updateHealthBar();
            if (!inNonIdleState && Time.time >= idleUntil && !death)
            {
                idleAnim = false;
                inNonIdleState = true;
                transform.localScale = new Vector3(player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1, 1, 1);
                float playerDist = Vector3.Distance(player.transform.position, transform.GetChild(0).position);
                float randomNum = Random.Range(0, 100);
                Coroutine c;
                if (playerDist > 5)
                {
                    if (randomNum < 50)
                    {
                        c = StartCoroutine(StompState());
                    }
                    else
                    {
                        c = StartCoroutine(ApproachState());
                    }
                }
                else if (playerDist < 3)
                {
                    if (randomNum < 15)
                    {
                        c = StartCoroutine(StepBackState());
                    }
                    else if (randomNum < 50)
                    {
                        c = StartCoroutine(SlashState());
                    }
                    else if (randomNum < 80)
                    {
                        c = StartCoroutine(DuckAttackState());
                    }
                    else if(randomNum < 90)
                    {
                        c = StartCoroutine(SlashDuckState());
                    }
                    else
                    {
                        c = StartCoroutine(DoubleSlashState());
                    }
                }
                else
                {
                    if (randomNum < 30)
                    {
                        c = StartCoroutine(DoubleSlashState());
                    }
                    else if (randomNum < 55)
                    {
                        c = StartCoroutine(StepThenDuckAttackState());
                    }
                    else if(randomNum < 80)
                    {
                        c = StartCoroutine(StepThenSlashState());
                    }
                    else
                    {
                        c = StartCoroutine(StompState());
                    }
                }
                currentCoroutines.Add(c);
                inNonIdleState = true;
            }
            else if (!inNonIdleState && !death)
            {
                if (!idleAnim)
                {
                    int randomNum = Random.Range(0, 100);
                    if(randomNum < 60)
                    {
                        anim.SetTrigger("drool");
                    }
                    else if(randomNum < 80)
                    {
                        anim.SetTrigger("breath");
                    }
                    else
                    {
                        anim.SetTrigger("blink");
                    }
                    idleAnim = true;
                }
            }
            yield return null;
        }
    }
    
    IEnumerator StepBackState()
    {
        inNonIdleState = true;
        anim.SetTrigger("stepBack");
        int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        step = true;
        while (step)
        {
            rb.velocity = new Vector2(-dir * moveSpeed, rb.velocity.y);
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        idleUntil = Time.time;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator SlashState()
    {
        inNonIdleState = true;
        slashing = true;
        anim.SetTrigger("slash");
        while (slashing)
        {
            yield return null;
        }
        idleUntil = Time.time + 1.5f;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator DuckAttackState()
    {
        inNonIdleState = true;
        duckAttacking = true;
        anim.SetTrigger("duckAttack");
        while (duckAttacking)
        {
            yield return null;
        }
        idleUntil = Time.time + 2.5f;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator DoubleSlashState()
    {
        doubleSlashing = true;
        inNonIdleState = true;
        anim.SetTrigger("doubleSlash");
        while (doubleSlashing)
        {
            yield return null;
        }
        idleUntil = Time.time + 3;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator StepThenDuckAttackState()
    {
        inNonIdleState = true;
        int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        walking = true;
        anim.SetBool("walking", true);
        step = true;
        while (step)
        {
            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
            yield return null;
        }
        anim.SetBool("walking", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
        duckAttacking = true;
        anim.SetTrigger("duckAttack");
        while (duckAttacking)
        {
            yield return null;
        }
        idleUntil = Time.time + 2.5f;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator StepThenSlashState()
    {
        inNonIdleState = true;
        int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        walking = true;
        anim.SetBool("walking", true);
        step = true;
        while (step)
        {
            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
            yield return null;
        }
        anim.SetBool("walking", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
        slashing = true;
        anim.SetTrigger("slash");
        while (slashing)
        {
            yield return null;
        }
        idleUntil = Time.time + 1.5f;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator StompState()
    {
        inNonIdleState = true;
        stomping = true;
        anim.SetTrigger("stomp");
        while (stomping)
        {
            yield return null;
        }
        idleUntil = Time.time + 2;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator SlashDuckState()
    {
        inNonIdleState = true;
        slashDucking = true;
        anim.SetTrigger("slashDuck");
        while (slashDucking)
        {
            yield return null;
        }
        idleUntil = Time.time + 2;
        inNonIdleState = false;
        yield return null;
    }
    IEnumerator ApproachState()
    {
        inNonIdleState = true;
        walking = true;
        anim.SetBool("walking", true);
        while(Mathf.Abs(player.transform.position.x - transform.GetChild(0).position.x) > 5)
        {
            int dir = player.transform.position.x > transform.GetChild(0).position.x ? 1 : -1;
            transform.localScale = new Vector3(dir, 1, 1);
            rb.velocity = new Vector2(dir, rb.velocity.y);
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        walking = false;
        anim.SetBool("walking", false);
        idleUntil = Time.time;
        inNonIdleState = false;
        yield return null;
    }
    public void EndStep()
    {
        step = false;
    }
    public void EndSlashing()
    {
        slashing = false;
    }
    public void EndDuckAttack()
    {
        duckAttacking = false;
    }
    public void EndStomp() {
        stomping = false;
    }
    public void EndDoubleSlash()
    {
        doubleSlashing = false;
    }
    public void EndSlashDuck()
    {
        slashDucking = false;
    }
    public void EndIdleAnim()
    {
        idleAnim = false;
    }        
    

    protected override void TriggerDeathStart()
    {
        endAllCoroutines();
        anim.SetTrigger("death");
        anim.SetBool("dead", true);
        Destroy(collisionBox);
        Destroy(rb);
        Destroy(healthBar);
    }
    public void DestroyIn3Sec()
    {
        Destroy(gameObject, 3);
    }
    public void StartDestroy()
    {
        Destroy(gameObject);
    }
    void endAllCoroutines()
    {
        StopAllCoroutines();
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
}
