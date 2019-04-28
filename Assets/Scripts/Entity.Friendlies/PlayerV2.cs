using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAbilities))]
[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(PlayerResources))]
public class PlayerV2 : MonoBehaviour
{
    #region Script References
    public PlayerControls controls;
    public PlayerPhysics physics;
    public PlayerAnimation anim;
    public PlayerAbilities abilities;
    public PlayerResources resources;
    #endregion

    #region State variables
    //player boolean variables
    public bool falling;
    public bool jumping;
    public bool shooting;
    public bool canUseAbility;
    public bool wallShooting;
    public bool wallGrabbing;
    public bool crouching;
    public bool facingRight;
    public bool blinking;
    public bool death;
    public bool invincible;
    public bool inHitStun;
    #endregion

    #region Wall grab variables
    public bool wallGrabLeft;
    public bool wallGrabRight;
    #endregion

    #region Player variables
    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;
    #endregion

    #region Scripting variables
    public float lastUpdateTime;
    #endregion

    private void Awake()
    {
        controls = GetComponent<PlayerControls>();
        physics = GetComponent<PlayerPhysics>();
        anim = GetComponent<PlayerAnimation>();
        abilities = GetComponent<PlayerAbilities>();
        resources = GetComponent<PlayerResources>();
    }
    private void Start()
    {
        lastUpdateTime = Time.time;
        StartCoroutine(UpdateLoop());
    }
    public void TriggerDeath()
    {
        death = true;
        abilities.TriggerDeath();
        physics.TriggerDeath();
        anim.TriggerDeath();
    }
    IEnumerator UpdateLoop()
    {
        while (true)
        {
            if (!GameStateController.paused)
            {
                if (!death)
                {
                    yield return StartCoroutine(physics.UpdatePhysics());
                    if(!inHitStun) yield return StartCoroutine(abilities.UpdateAbilities());
                }
                yield return StartCoroutine(resources.UpdateResources());
                yield return StartCoroutine(anim.UpdateAnimation());
                yield return StartCoroutine(controls.UpdateControls());
                lastUpdateTime = Time.time;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
}
