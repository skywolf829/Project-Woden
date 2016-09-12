
using UnityEngine;
using System.Collections;

public class BishopHeal : MonoBehaviour
{
    private string lastTypeHitBy;
    string type = "heal";
    private bool hitOnce;
    private int damage = -400;

    public Collider2D hitBox;

    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 0.75f);
    }
    public void applyDamage(int d)
    {

    }
    public void hitBy(string t)
    {
        lastTypeHitBy = t;
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Enemy" && !hitOnce)
        {
            c.gameObject.BroadcastMessage("hitBy", type);
            c.gameObject.BroadcastMessage("applyDamage", damage);            
            hitOnce = true;
        }

    }

}
