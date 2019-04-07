using System.Collections;
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
        sr.flipX = p.wallGrabbing;
        yield return null;
    }
    public void StartJump()
    {
        anim.SetTrigger("startJump");
    }
    public void StartShooting()
    {
        anim.SetTrigger("startShooting");
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
}
