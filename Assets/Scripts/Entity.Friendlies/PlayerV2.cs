using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAbilities))]
[RequireComponent(typeof(PlayerInventory))]
public class PlayerV2 : MonoBehaviour
{
    #region Script References
    public PlayerControls controls;
    public PlayerPhysics physics;
    public PlayerAnimation anim;
    public PlayerAbilities abilities;
    #endregion

    #region State variables
    //player boolean variables
    public bool falling;
    public bool jumping;
    public bool shooting;
    public bool wallShooting;
    public bool wallGrabbing;
    public bool crouching;
    public bool facingRight;
    #endregion

    #region Wall grab variables
    public bool wallGrabLeft;
    public bool wallGrabRight;
    #endregion

    #region Player variables
    private int health;
    private int maxHealth;
    private int mana;
    private int maxMana;
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
    }
    private void Start()
    {
        lastUpdateTime = Time.time;
        StartCoroutine(UpdateLoop());
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            if (!GameStateController.paused)
            {
                yield return StartCoroutine(physics.UpdatePhysics());
                yield return StartCoroutine(abilities.UpdateAbilities());
                yield return StartCoroutine(anim.UpdateAnimation());
                yield return StartCoroutine(controls.UpdateControls());
                lastUpdateTime = Time.time;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
}
