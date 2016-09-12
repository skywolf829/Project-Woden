using UnityEngine;
using System.Collections;

public class SarrakkenMap : MonoBehaviour {
    GameObject Player;
    GameObject Camera;
    GameObject TrapDoor1;
    GameObject TrapDoor2;
    GameObject collision1;
    GameObject collision2;
    float xTrigger;
    float xTriggerTime;
    float xTriggerLength;
    float xTrigger2Length;
    bool triggered;
    bool finished;
	GameObject sarrakken;
    void Start()
	{
		xTrigger = this.transform.position.x;
		Player = GameObject.Find ("Player");
		TrapDoor1 = GameObject.Find ("Door_02");
		TrapDoor2 = GameObject.Find ("Door2_03");
		collision1 = GameObject.FindGameObjectWithTag ("event1");
		collision2 = GameObject.FindGameObjectWithTag ("event2");
		Camera = GameObject.Find ("Camera");
		sarrakken = GameObject.Find ("Sarrakken");
		if (TrapDoor1) {
			TrapDoor1.SetActive (false);
		} else {
			Debug.Log ("no TrapDoor1");
		}
		if (TrapDoor2) {
			TrapDoor2.SetActive (false);
		} else {
			Debug.Log ("no TrapDoor2");
		}
		if (collision2) {
			collision2.SetActive (false);
		} else {
			Debug.Log ("No collision2");
		}

        xTriggerLength = 0.7f;
        xTrigger2Length = 0.7f;
    }
    void Update()
    {
        if (!finished)
        {

            if (Player.transform.position.x > xTrigger && !triggered && TrapDoor1 != null)
            {
                TrapDoor1.SetActive(true);
                collision1.SetActive(false);
                collision2.SetActive(true);
                xTriggerTime = Time.time;
                triggered = true;
            }
            if (triggered && Time.time > xTriggerTime + xTriggerLength + xTrigger2Length && Camera != null)
            {
                if (Camera != null)
                {
                    Camera.BroadcastMessage("SetFixed");
                }
                Destroy(this.gameObject, 2.0f);
                finished = true;
            }
            else if (triggered && Time.time > xTriggerTime + xTriggerLength && TrapDoor2 != null)
            {
                TrapDoor2.SetActive(true);
				if(sarrakken != null){
					sarrakken.BroadcastMessage("startEncounter");
				}

            }
        }
        
    }
}

