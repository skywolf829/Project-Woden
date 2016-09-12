using UnityEngine;
using System.Collections;

public class Sarrakken : Enemy {

	private bool shooting, groundSlam, greenPhase, yellowPhase, redPhase;
	private int cycle;

	private float progressCycle, shootingTimer;
	private float preShootAnimationLength, postShootAnimationLength, groundSlamAnimationLength,
	waitTime;
	private float waterBlastSpeed;
	private float beamAngle;

	private float waterBlastInterval;

    private int maxRedHealth, maxYellowHealth, maxGreenHealth;
	private int greenHealth, yellowHealth, redHealth;
	private int lastGreenHealth, lastYellowHealth, lastRedHealth;
	private int waterBlastCount, beamCount, armCount, lockingCycles, beamBlastCount;

    public GameObject WaterBlast, blocker, spike;
	public GameObject BeamBlast;
	public GameObject Cherry;
	public GameObject Banshee;
	public GameObject walls;
	private GameObject blocker1, blocker2, blocker3;


    // Use this for initialization
    protected override void Start () {
        base.Start();
		deathAnimationLength = 0.43f;
        moveSpeed = 0.6f;
		cycle = 0;
        maxSpeed = 8.5f;
        stopSpeed = 0.1f;
        maxFallSpeed = 8.0f;
		preShootAnimationLength = 0.33f;
		postShootAnimationLength = 0.33f;
		groundSlamAnimationLength = 0.56f;
		waitTime = 3.0f;
        health = maxHealth = 1600;
		waterBlastSpeed = 7.0f;
		beamAngle = -5.0f;
        damage = 0;
		waterBlastInterval = 0.5f;
		range = 20;
		maxRedHealth = maxYellowHealth = maxGreenHealth = greenHealth = redHealth = yellowHealth = 5000;
        weakness = "light";
		strength = "water";
		facingRight = true;
		Destroy (healthBar);
		Flip ();
    }

	protected override void AI()
    {
		if (aggro) {
			if (greenPhase) {
				if (cycle == 0) {
					if (!shooting) {
						shooting = true;
						progressCycle = Time.time + preShootAnimationLength;
					} else if (Time.time > progressCycle) {
						cycle++;
						shootingTimer = Time.time;
					}
				} else if (cycle == 1) {
					if (Time.time > shootingTimer) {
						if (WaterBlast) {
							StartCoroutine (createRandomProjectile (WaterBlast, waterBlastSpeed, 0, transform.position.x - 1.0f, transform.position.y, 20.0f));
							StartCoroutine (createRandomProjectile (WaterBlast, waterBlastSpeed, 0, transform.position.x - 1.0f, transform.position.y, 20.0f));
						}
						shootingTimer += waterBlastInterval;
						waterBlastCount++;
					} else if (waterBlastCount >= 9) {
						waterBlastCount = 0;
						shooting = false;
						cycle++;
						progressCycle = Time.time + postShootAnimationLength;
					} 
				} else if (cycle == 2) {
					if (Time.time > progressCycle) {
						cycle++;
						groundSlam = true;
						progressCycle += groundSlamAnimationLength;
					}
				} else if (cycle == 3) {
					if (Time.time > progressCycle) {
						cycle++;
						progressCycle += waitTime;
					}
				} else if (cycle == 4) {
					if (blocker) {
						createblockers ();
					}
					cycle++;
					groundSlam = false;
					progressCycle = Time.time + 2.0f;
				} else if (cycle == 5) {
					if (Time.time > progressCycle) {
						cycle++;
					}
				} else if (cycle == 6) {
					if (!shooting) {
						shooting = true;
						progressCycle = Time.time + preShootAnimationLength;
					} else if (Time.time > progressCycle) {
						cycle++;
						shootingTimer = Time.time;
					}
				} else if (cycle == 7) {
					if (Time.time > shootingTimer) {
						if (WaterBlast) {
							StartCoroutine (createRandomProjectile (WaterBlast, waterBlastSpeed, 0, transform.position.x, transform.position.y, 20.0f));
							StartCoroutine (createRandomProjectile (WaterBlast, waterBlastSpeed, 0, transform.position.x, transform.position.y, 20.0f));
						}
						shootingTimer += waterBlastInterval;
						waterBlastCount++;
					} else if (waterBlastCount >= 9) {
						waterBlastCount = 0;
						shooting = false;
						cycle++;
						progressCycle = Time.time + postShootAnimationLength + 3.0f;
					} 
				} else if (cycle == 8) {
					if (Time.time > progressCycle) {
						cycle = 0;
					}
				}
			} else if (yellowPhase) {
				if (cycle == 0) {
					if (!shooting) {
						shooting = true;
						progressCycle = Time.time + preShootAnimationLength;
					} else if (Time.time > progressCycle) {
						cycle++;
						shootingTimer = Time.time;
					}
				} else if (cycle == 1) {
					if (Time.time > shootingTimer) {
						if (WaterBlast) {
							StartCoroutine (createRandomProjectile (WaterBlast, waterBlastSpeed, 0, transform.position.x - 1.0f, transform.position.y, 40.0f));
							StartCoroutine (createRandomProjectile (WaterBlast, waterBlastSpeed, 0, transform.position.x - 1.0f, transform.position.y, 40.0f));
						}
						shootingTimer += waterBlastInterval / 2.0f;
						waterBlastCount++;
					} else if (waterBlastCount >= 9) {
						waterBlastCount = 0;
						shooting = false;
						cycle++;
						progressCycle = Time.time + postShootAnimationLength;
					} 
				} else if (cycle == 2) {
					if (Time.time > progressCycle) {
						cycle++;
						groundSlam = true;
						progressCycle += groundSlamAnimationLength;
					}
				} else if (cycle == 3) {
					if (Time.time > progressCycle) {
						cycle++;
						progressCycle += waitTime;
					}
				} else if (cycle == 4) {
					if (blocker) {						
						createblockers ();
					}
					cycle++;
					groundSlam = false;
					progressCycle = Time.time + 2.0f;
				} else if (cycle == 5) {
					if (Time.time > progressCycle) {
						cycle++;
					}
				} else if (cycle == 6) {
					if (!shooting) {
						shooting = true;
						progressCycle = Time.time + preShootAnimationLength;
					} else if (Time.time > progressCycle) {
						cycle++;
						shootingTimer = Time.time;
					}
				} else if (cycle == 7) {
					if (Time.time > shootingTimer) {
						if (BeamBlast) {
							StartCoroutine (createAngledProjectile (BeamBlast, waterBlastSpeed, 0, transform.position.x, transform.position.y, beamAngle));
						}
						shootingTimer += waterBlastInterval / 5;
						beamAngle += 1.0f;
						beamBlastCount++;
					} else if (beamBlastCount >= 15) {
						beamBlastCount = 0;
						shooting = false;
						cycle++;
						beamAngle = -5.0f;
						progressCycle = Time.time + postShootAnimationLength + 3.0f;
					} 
				} else if (cycle == 8) {
					if (Time.time > progressCycle) {
						cycle = 0;
					}
				}
			} else if (redPhase) {
				walls.SetActive (true);
			}
		}
    }
	protected override void Update ()
	{
		AI();
		updateAnimation();
		checkHit();
	}
	public void startEncounter(){
		aggro = true;
		greenPhase = true;
	}
    public override void applyDamage(int d)
    {
		if (lastTypeHitBy.Equals (weakness)) {
			d *= 4;
		} else if (lastTypeHitBy.Equals (strength)) {
			d /= 4;
		} 
        if (greenPhase) {
			greenHealth -= d;
			if(greenHealth <= 0){
				greenHealth = 0;
				greenPhase = false;
				yellowPhase = true;
				shooting = false;
				groundSlam = false;
			}
		} else if (yellowPhase) {
			yellowHealth -= d;
			if(yellowHealth <= 0){
				yellowHealth = 0;
				yellowPhase = false;
				redPhase = true;
				shooting = false;
				groundSlam = false;
			}
		} else if (redPhase) {
			redHealth -= d;
			if(redHealth <= 0){
				redHealth = 0;
				death = true;
				redPhase = false;
				shooting = false;
				groundSlam = false;
			}
		} else { 
			//Debug.Log ("Dead");
		}
    }
	private void createblockers(){
		if (!blocker1) {
			blocker1 = GameObject.Instantiate<GameObject> (blocker);
			if (blocker1 != null) {
				blocker1.transform.tag = "Enemy";
				blocker1.transform.position = new Vector2 (transform.position.x - 1, transform.position.y - 0.5f);
				blocker1.BroadcastMessage ("timeContracted", 1);
			}
		}
		if (!blocker2) {
			blocker2 = GameObject.Instantiate<GameObject> (blocker);
			if (blocker2 != null) {
				blocker2.transform.tag = "Enemy";

				blocker2.transform.position = new Vector2 (transform.position.x - 2, transform.position.y - 0.5f);

				blocker2.BroadcastMessage ("timeContracted", 2);
			}
		}
		if (!blocker3) {
			blocker3 = GameObject.Instantiate<GameObject> (blocker);
			if (blocker3 != null) {
				blocker3.transform.tag = "Enemy";
				blocker3.transform.position = new Vector2 (transform.position.x - 3, transform.position.y - 0.5f);

				blocker3.BroadcastMessage ("timeContracted", 3);
			}
		}
	}
	protected override void updateAnimation()
    {
		anim.SetBool ("shooting", shooting);
		anim.SetBool ("groundslam", groundSlam);
		anim.SetBool ("death", death);
    }
	protected override void updateHealthBar(){

		
	}
	protected void OnGUI(){
		if (aggro) {
			if (greenPhase) {
				drawHealthBar((int)(Screen.width * (1.0 / 10.0)), (int)((8.5/10.0) * Screen.height), 
					(int)(Screen.width * (8.0 / 10.0) * yellowHealth / maxYellowHealth), (int)(Screen.height * (1.0 / 10.0)), 255, 255, 0);
				drawHealthBar((int)(Screen.width * (1.0 / 10.0)), (int)((8.5/10.0) * Screen.height), 
					(int)(Screen.width * (8.0 / 10.0) * greenHealth / maxGreenHealth), (int)(Screen.height * (1.0 / 10.0)), 0, 255, 0);
			} else if (yellowPhase) {
				drawHealthBar((int)(Screen.width * (1.0 / 10.0)), (int)((8.5/10.0) * Screen.height), 
					(int)(Screen.width * (8.0 / 10.0) * redHealth / maxRedHealth), (int)(Screen.height * (1.0 / 10.0)), 255, 0, 0);
				drawHealthBar((int)(Screen.width * (1.0 / 10.0)), (int)((8.5/10.0) * Screen.height), 
					(int)(Screen.width * (8.0 / 10.0) * yellowHealth / maxYellowHealth), (int)(Screen.height * (1.0 / 10.0)), 255, 255, 0);
			} else if (redPhase) {
				drawHealthBar((int)(Screen.width * (1.0 / 10.0)), (int)((8.5/10.0) * Screen.height), 
					(int)(Screen.width * (8.0 / 10.0) * redHealth / maxRedHealth), (int)(Screen.height * (1.0 / 10.0)), 255, 0, 0);
			}
			              
		}
	}

	private void drawHealthBar(int x, int y, int w, int h, float r, float g, float b){
		Color color = new Color (r, g, b);
		Rect position = new Rect (x, y, w, h);
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}

    
}
