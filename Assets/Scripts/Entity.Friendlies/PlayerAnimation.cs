using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerV2))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerV2 p;
    SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        p = GetComponent<PlayerV2>();
        sr = GetComponent<SpriteRenderer>();
    }
    public IEnumerator UpdateAnimation()
    {
        anim.SetFloat("speed", GetComponent<Rigidbody2D>().velocity.x);
        anim.SetBool("falling", p.falling && GetComponent<Rigidbody2D>().velocity.y < 0);
        anim.SetBool("crouching", p.crouching);
        anim.SetBool("jumping", p.jumping);
        anim.SetBool("wallGrabbing", p.wallGrabbing);
        anim.SetBool("shooting", p.shooting);
        sr.flipX = p.wallGrabbing;
        yield return null;
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
