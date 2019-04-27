using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBurstScript : EnemyProjectile
{
    int attackNo = 0;
    int dir = 0;
    // Start is called before the first frame update
    void Start()
    {
        damage = 20;
        type = "fire";
    }
    public void SetAttack(int num)
    {
        attackNo = num;
    }
    public void SetDir(int d)
    {
        dir = d;
    }
    // Update is called once per frame
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
    public void TriggerDestroy()
    { 
        if(attackNo < 10)
        {
            RaycastHit2D[] rchs = Physics2D.RaycastAll(transform.GetChild(0).position, new Vector3(dir, 0, 0));
            List<Vector3> wallContactPoints = new List<Vector3>();
            for(int i = 0; i < rchs.Length; i++)
            {
                if(rchs[i].collider != null && rchs[i].collider.tag == "Map") {
                    wallContactPoints.Add(rchs[i].point);
                } 
            }
            if(wallContactPoints.Count > 0)
            {
                float minDist = float.PositiveInfinity;
                for(int i = 0; i < wallContactPoints.Count; i++)
                {
                    if(Vector3.Distance(transform.GetChild(0).position, wallContactPoints[i]) < minDist)
                    {
                        minDist = Vector3.Distance(transform.GetChild(0).position, wallContactPoints[i]);
                    }
                }
                if(minDist > 1)
                {
                    SpawnNewFlameBurst();
                }
            }
            else{
                SpawnNewFlameBurst();
            }
        }
        Destroy(gameObject);
    }
    void SpawnNewFlameBurst()
    {
        Vector3 possiblePosition = transform.GetChild(0).position + new Vector3(dir, 0, 0);

        RaycastHit2D[] rchs = Physics2D.RaycastAll(possiblePosition, new Vector3(0, -1, 0));
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
                GameObject g = Instantiate(gameObject);
                g.transform.position = possiblePosition - new Vector3(0, minDist -
                    Vector3.Distance(g.transform.position, g.transform.GetChild(1).position), 0);
                g.GetComponent<FlameBurstScript>().SetAttack(attackNo + 1);
                g.GetComponent<FlameBurstScript>().SetDir(dir);
            }
        }
        
    }
}
