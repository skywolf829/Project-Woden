using UnityEngine;
using System.Collections;

public class Battery : Enemy {
    
    private bool batteryState;
    private bool batteryDeath;

    private float batteryDeathTime;
    public GameObject battery;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        Destroy(healthBar);
        batteryDeathTime = 0.43f;
        anim.Play("SparkDying1");
        death = true;
        batteryDeath = false;
        aggro = false;
        range = 0;
    }	
    
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        if (!batteryDeath)
        {
            if (c.gameObject.name == "Player")
            {
                if (battery)
                {
                    //GameObject.FindGameObjectWithTag("Inventory").BroadcastMessage("addItem", battery);
                }

                batteryDeath = true;
                Destroy(gameObject, batteryDeathTime);
            }
            if (c.gameObject.name.StartsWith("Spark") || c.gameObject.name.StartsWith("Circuit"))
            {
                c.BroadcastMessage("upgrade", gameObject);
            }
        }
    }

    public void taken()
    {
        if (!batteryDeath)
        {
            batteryDeath = true;
            Destroy(gameObject, batteryDeathTime);
        }
    }
    protected override void updateAnimation()
    {        
		anim.SetBool ("aggro", aggro);
		anim.SetBool ("batteryDeath", batteryDeath);
		anim.SetBool ("death", death);
    }
    
}
