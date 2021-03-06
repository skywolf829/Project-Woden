﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerV2))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerV2 p;
    SpriteRenderer sr;
    Rigidbody2D rb;

    public bool inShot2Window = false;
    public bool inShot3Window = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        p = GetComponent<PlayerV2>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    public IEnumerator UpdateAnimation()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("falling", p.falling && rb.velocity.y < 0);
        anim.SetBool("crouching", p.crouching);
        anim.SetBool("jumping", p.jumping || rb.velocity.y > 0);
        anim.SetBool("wallGrabbing", p.wallGrabbing);
        anim.SetBool("shooting", p.shooting);
        anim.SetBool("isDead", p.death);
        sr.flipX = p.wallGrabbing;
        yield return null;
    }
    public void StartJump()
    {
        anim.SetTrigger("startJump");
    }
    public void StartBlink()
    {
        anim.SetTrigger("startBlink");
    }
    public void StartShooting()
    {
        anim.SetTrigger("startShooting");
    }
    public void StartShooting2()
    {
        anim.SetTrigger("startShooting2");
    }
    public void StartShooting3()
    {
        anim.SetTrigger("startShooting3");
    }
    public void StartChargedAttack()
    {
        anim.SetTrigger("startChargedAttack");
    }
    public void StartCrouch()
    {
        anim.SetTrigger("startCrouch");
    }
    public void StartFalling()
    {
        anim.SetTrigger("startFalling");
    }
    public void StartWallGrab()
    {
        anim.SetTrigger("startWallGrab");
    }
    public void SetHitStun()
    {
        anim.SetTrigger("hitStun");
    }
    public void TriggerDeath()
    {
        anim.SetTrigger("death");
        anim.SetBool("isDead", true);
        anim.SetBool("shooting", false);
        anim.SetBool("falling", false);
        anim.SetBool("crouching", false);
        anim.SetBool("wallGrabbing", false);
        anim.SetBool("channeling", false);
        anim.SetBool("parry", false);
        anim.SetBool("dodge", false);
        anim.ResetTrigger("hitStun");
        anim.ResetTrigger("startShooting2");
        anim.ResetTrigger("startShooting3");
        anim.ResetTrigger("startWallGrab");
        anim.ResetTrigger("startCrouch");
        anim.ResetTrigger("startShooting");
        anim.ResetTrigger("startFalling");
        anim.ResetTrigger("startJump");
    }
    public void FaceLeft()
    {
        transform.localScale = new Vector3(transform.localScale.x < 0 ? transform.localScale.x : -1 * transform.localScale.x, 
            transform.localScale.y, transform.localScale.z);
    }
    public void FaceRight()
    {
        transform.localScale = new Vector3(transform.localScale.x > 0 ? transform.localScale.x : -1 * transform.localScale.x,
            transform.localScale.y, transform.localScale.z);
    }
    public void ShootingLock()
    {
        p.canUseAbility = false;
    }
    public void ReleaseShootingLock()
    {
        p.canUseAbility = true;
    }
    public void StartWindowForShot2()
    {
        inShot2Window = true;
    }
    public void EndWindowForShot2()
    {
        inShot2Window = false;
    }
    public void StartWindowForShot3()
    {
        inShot3Window = true;
    }
    public void EndWindowForShot3()
    {
        inShot3Window = false;
    }
    public void ResetAnimation()
    {
        anim.SetTrigger("Reset");
    }
}
