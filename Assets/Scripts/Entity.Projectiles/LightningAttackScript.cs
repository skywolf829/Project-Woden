using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttackScript : EnemyProjectile
{
    private int attackNo = 0;
    private bool heavy = false;

    bool initDestroy = false;
    // Start is called before the first frame update
    void Awake()
    {
        damage = 50;
        moveSpeed = 0;
        type = "lightning";
    }
    public void SetAttack(int num = 0)
    {
        if (num != 0)
        {
            attackNo = num;
            GetComponent<Animator>().SetInteger("attackNumber", attackNo);
            damage = 50;
            if (num == 3) damage = 60;
        }
        else
        {
            heavy = true;
            GetComponent<Animator>().SetBool("heavyAttack", heavy);
            damage = 150;
        }
    }
    public void TriggerDestroy()
    {
        Destroy(gameObject);
    }
    protected override void Update()
    {

    }
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag != "Enemy" && c.gameObject.tag != "Boss" && c.gameObject.tag != "Projectile"
            && c.gameObject.tag != "EnemyProjectile" && c.gameObject.tag != "EnemyProjectile" && !hit)
        {
            hit = true;
            if (c.gameObject.tag == "Player")
            {
                c.BroadcastMessage("hitBy", type);
                c.BroadcastMessage("applyDamage", damage);
            }
        }
    }
}
