using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerV2))]
public class PlayerPhysics : MonoBehaviour
{
    BoxCollider2D box;
    Rigidbody2D rb;
    PlayerV2 p;

    //different max speeds and scalars
    public float jumpMaxSpeed = 7.5f;
    public float moveSpeed = 1.0f;
    public float airMoveSpeed = 0.6f;
    public float slowingSpeed = 0.3f;
    public float maxMoveSpeed = 7.0f;
    public float maxFallSpeed = 10.0f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        p = GetComponent<PlayerV2>();
    }

    public IEnumerator UpdatePhysics()
    {
        UpdateVerticalMovement();
        UpdateHorizontalMovement();

        yield return null;
    }

    void UpdateHorizontalMovement()
    {
        if (p.crouching) return;
        float dx = rb.velocity.x;
        int xInput = 0;
        xInput += p.controls.Intents.Contains(PlayerControls.IntentType.RIGHT) ? 1:0;
        xInput -= p.controls.Intents.Contains(PlayerControls.IntentType.LEFT) ? 1:0;

        if (p.wallGrabbing)
        {
            if(p.wallGrabLeft && xInput > 0)
            {
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                dx += xInput * moveSpeed * (Time.time - p.lastUpdateTime);
            }
            else if (p.wallGrabRight && xInput < 0)
            {
                p.wallGrabbing = false;
                p.wallGrabRight = false;
                dx += xInput * moveSpeed * (Time.time - p.lastUpdateTime);
            }
        }
        else if(p.falling || p.jumping)
        {
            dx += xInput * airMoveSpeed * (Time.time - p.lastUpdateTime);
        }
        else
        {
            if (dx != 0 && xInput == 0)
            {
                float ogdx = dx;
                dx = dx > 0 ?
                    dx - slowingSpeed * moveSpeed * (Time.time - p.lastUpdateTime) :
                    dx + slowingSpeed * moveSpeed * (Time.time - p.lastUpdateTime);
                if (ogdx > 0 && dx < 0 || ogdx < 0 && dx > 0) dx = 0;
            }
            else if (dx > 0 && xInput < 0)
            {
                dx = dx - moveSpeed * (Time.time - p.lastUpdateTime);
            }
            else if (dx < 0 && xInput > 0)
            {
                dx = dx + moveSpeed * (Time.time - p.lastUpdateTime);
            }
            else
            {
                dx += xInput * moveSpeed * (Time.time - p.lastUpdateTime);
            }
        }
        dx = Mathf.Clamp(dx, -maxMoveSpeed, maxMoveSpeed);
        rb.velocity = new Vector2(dx, rb.velocity.y);
    }

    void UpdateVerticalMovement()
    {
        if (p.crouching)
        {

        }
        if (p.jumping)
        {

        }
        if (p.wallGrabbing)
        {

        }
        if (p.falling)
        {

        }
        if(p.crouching && p.controls.Intents.Contains(PlayerControls.IntentType.CROUCH))
        {

        }
    }

    public void StartJump()
    {

    }

    public void StartCrouch()
    {

    }
}
