using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinShaman : EnemyV2
{
   
    
    public GameObject FlameBurst, LightningBall, LightningAttack;
    List<Coroutine> currentCoroutines = new List<Coroutine>();
    bool inNonIdleState = false;

    bool flamePunching, lightningAttacking, lightningPunching, staffRaiseFlaming, staffRaiseLightning, walking;

    float moveSpeed;
    float idleUntil = 0;

    string lastAttackType;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1;
        health = maxHealth = 1600;
        collisionDamage = 40;
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
            if (!inNonIdleState && Time.time > idleUntil)
            {
                transform.localScale = new Vector3(player.transform.position.x > transform.position.x ? 1: -1, 1, 1);
                float playerDist = Vector3.Distance(player.transform.position, transform.position);
                float randomNum = Random.Range(0, 100);
                Coroutine c;
                if (playerDist > 4)
                {
                    if(randomNum < 30)
                    {
                        c = StartCoroutine(LightningPunchState());
                    }
                    else if(randomNum < 60)
                    {
                        c = StartCoroutine(StaffRaiseFlameState());
                    }
                    else if(randomNum < 80)
                    {
                        c = StartCoroutine(LightningPunchState());
                    }
                    else
                    {
                        c = StartCoroutine(WalkingState());
                    }
                }
                else if(playerDist < 3)
                {
                    if (randomNum < 30)
                    {
                        c = StartCoroutine(FlamePunchState());
                    }
                    else if (randomNum < 60)
                    {
                        c = StartCoroutine(StaffRaiseFlameState());
                    }
                    else if(randomNum < 80)
                    {
                        c = StartCoroutine(LightningPunchState());
                    }
                    else
                    {
                        c = StartCoroutine(StaffRaiseLightningState());
                    }
                }
                else
                {
                    if (randomNum < 30)
                    {
                        c = StartCoroutine(LightningAttacksState());
                    }
                    else if (randomNum < 80)
                    {
                        c = StartCoroutine(FlamePunchState());
                    }
                    else
                    {
                        c = StartCoroutine(LightningPunchState());
                    }
                }
                currentCoroutines.Add(c);
                inNonIdleState = true;
            }
            yield return null;
        }
    }

    IEnumerator LightningAttacksState()
    {
        inNonIdleState = true;
        lightningAttacking = true;
        anim.SetTrigger("lightningAttack");
        while (lightningAttacking)
        {

            yield return null;
        }
        idleUntil = Time.time + 3;
        inNonIdleState = false;
    }
    IEnumerator FlamePunchState()
    {
        inNonIdleState = true;
        flamePunching = true;
        anim.SetTrigger("flamePunch");
        while (flamePunching)
        {

            yield return null;
        }
        idleUntil = Time.time + 1;
        inNonIdleState = false;
    }
    IEnumerator LightningPunchState()
    {
        inNonIdleState = true;
        lightningPunching = true;
        anim.SetTrigger("lightningPunch");
        while (lightningPunching)
        {

            yield return null;
        }
        idleUntil = Time.time + 2;
        inNonIdleState = false;
    }
    IEnumerator StaffRaiseFlameState()
    {
        inNonIdleState = true;
        staffRaiseFlaming = true;
        anim.SetTrigger("staffRaiseFlame");
        while (staffRaiseFlaming)
        {

            yield return null;
        }
        idleUntil = Time.time + 3.5f;
        inNonIdleState = false;
    }
    IEnumerator StaffRaiseLightningState()
    {
        inNonIdleState = true;
        staffRaiseLightning = true;
        anim.SetTrigger("staffRaiseLightning");
        while (staffRaiseLightning)
        {
            yield return null;
        }
        idleUntil = Time.time + 3;
        inNonIdleState = false;
    }
    IEnumerator WalkingState()
    {
        inNonIdleState = true;
        walking = true;
        anim.SetTrigger("walking");
        while (walking)
        {
            int dir = transform.position.x > player.transform.position.x ? -1 : 1;
            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
            yield return null;
        }

        idleUntil = Time.time;
        inNonIdleState = false;
    }

    public void CastFlameBurst(int fromStaff)
    {
        Vector3 possiblePosition = (fromStaff > 0 ? player.transform.GetChild(0).position : transform.GetChild(1).position);

        RaycastHit2D[] rchs = Physics2D.RaycastAll(new Vector2(possiblePosition.x, possiblePosition.y), new Vector2(0, -1));
        List<Vector3> groundContactPoints = new List<Vector3>();
        for (int i = 0; i < rchs.Length; i++)
        {
            if (rchs[i].collider != null && rchs[i].collider.tag == "Map")
            {
                groundContactPoints.Add(rchs[i].point);
            }
        }

        if (groundContactPoints.Count > 0)
        {
            float minDist = float.PositiveInfinity;
            for (int i = 0; i < groundContactPoints.Count; i++)
            {
                if (Vector3.Distance(possiblePosition, groundContactPoints[i]) < minDist)
                {
                    minDist = Vector3.Distance(possiblePosition, groundContactPoints[i]);
                }
            }
            if (minDist < 10)
            {
                GameObject g = Instantiate(FlameBurst);
                g.GetComponent<FlameBurstScript>().SetAttack(fromStaff > 0 ? 10 : 1);
                g.GetComponent<FlameBurstScript>().SetDir((transform.localScale.x > 0 ? 1 : -1));
                g.transform.position = possiblePosition - new Vector3(0, minDist - 1, 0);
                if(fromStaff == 3)
                {
                    g.transform.position -= new Vector3(0.5f, 0, 0);
                    GameObject g2 = Instantiate(FlameBurst);
                    g2.GetComponent<FlameBurstScript>().SetAttack(10);
                    g2.GetComponent<FlameBurstScript>().SetDir((transform.localScale.x > 0 ? 1 : -1));
                    g2.transform.position = g.transform.position + new Vector3(1, 0, 0);
                }
            }
        }
        lastAttackType = "fire";
    }
    public void CastLightningBall(int fromStaff)
    {
        GameObject g = Instantiate(LightningBall);
        g.transform.position = fromStaff > 0 ?
            transform.GetChild(0).position :
            transform.GetChild(1).position;
        g.SendMessage("SetTarget", player.transform.position - new Vector3(0, 0.5f, 0));
        lastAttackType = "lightning";
    }
    public void CastLightningAttack(int num)
    {
        GameObject g = Instantiate(LightningAttack);        
        g.transform.position = transform.GetChild(2).position + new Vector3(0, 1, 0) 
            + (transform.localScale.x > 0 ? new Vector3(3, 0) : new Vector3(-3, 0));
        g.SendMessage("SetAttack", num);
        lastAttackType = "lightning";
    }
    public void EndLightningAttack()
    {
        lightningAttacking = false;
    }
    public void EndFlamePunch()
    {
        flamePunching = false;
    }
    public void EndLightningPunch()
    {
        lightningPunching = false;
    }
    public void EndStaffRaiseFlame()
    {
        staffRaiseFlaming = false;
    }
    public void EndStaffRaiseLightning()
    {
        staffRaiseLightning = false;
    }
    public void EndWalking()
    {
        walking = false;
        rb.velocity = new Vector3(0, 0, 0);
    }
    protected override void TriggerDeathStart()
    {
        endAllCoroutines();
        if (lastAttackType == "fire")
            anim.SetTrigger("fireDeath");
        else anim.SetTrigger("lightningDeath");
        Destroy(collisionBox);
        Destroy(rb);
    }
    public void DestroyAfterLightningDeath()
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
}
