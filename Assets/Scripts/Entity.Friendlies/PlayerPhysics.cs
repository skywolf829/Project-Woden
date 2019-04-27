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

    public bool DEBUG = false;

    //different max speeds and scalars
    public float jumpMaxSpeed = 7.5f;
    public float moveSpeed = 1.0f;
    public float airMoveSpeed = 0.6f;
    public float airResistance = 0.15f;
    public float slowingSpeed = 0.3f;
    public float maxMoveSpeed = 7.0f;
    public float maxFallSpeed = 10.0f;
    public float jumpHoldLength = 0.1f;
    public float jumpGracePeriod = 0.1f;
   

    public float normalGravity = 2.8f;
    public float wallGrabGravity = 0.2f;


    float jumpStartTime = 0;
    float wallLeaveTime = 0;
    bool justLeftWallLeft, justLeftWallRight;
    public bool touchingBottom, touchingTop, touchingLeft, touchingRight;

    ContactPoint2D[] currentContactPoints;
    List<GameObject> contactPointObjects;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        p = GetComponent<PlayerV2>();

        currentContactPoints = new ContactPoint2D[0];
        contactPointObjects = new List<GameObject>();
    }

    public IEnumerator UpdatePhysics()
    {        
        UpdateVerticalMovement();
        UpdateHorizontalMovement();
        

        if (DEBUG)
        {
            while (contactPointObjects.Count < currentContactPoints.Length)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.localScale = Vector3.one * 0.1f;
                g.GetComponent<Renderer>().material.color = Color.yellow;
                g.name = "Contact Point Visual";
                contactPointObjects.Add(g);
            }
            while(contactPointObjects.Count > currentContactPoints.Length)
            {
                GameObject g = contactPointObjects[contactPointObjects.Count - 1];
                contactPointObjects.RemoveAt(contactPointObjects.Count - 1);
                Destroy(g);
            }
            for(int i = 0; i < currentContactPoints.Length; i++)
            {
                contactPointObjects[i].transform.position = currentContactPoints[i].point;
            }
        }
        yield return null;
    }

    void UpdateHorizontalMovement()
    {
        float dx = rb.velocity.x;
        int xInput = 0;
        int lastXInput = 0;
        xInput += p.controls.Intents.Contains(PlayerControls.IntentType.RIGHT) ? 1:0;
        xInput -= p.controls.Intents.Contains(PlayerControls.IntentType.LEFT) ? 1:0;
        lastXInput += p.controls.PreviousIntents.Contains(PlayerControls.IntentType.RIGHT) ? 1 : 0;
        lastXInput -= p.controls.PreviousIntents.Contains(PlayerControls.IntentType.LEFT) ? 1 : 0;

        if (p.wallGrabbing)
        {
            if(p.wallGrabLeft && xInput > 0 && lastXInput <= 0)
            {
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                justLeftWallLeft = true;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                dx += xInput * moveSpeed * (Time.time - p.lastUpdateTime);
            }
            else if (p.wallGrabRight && xInput < 0 && lastXInput >= 0)
            {
                p.wallGrabbing = false;
                p.wallGrabRight = false;
                justLeftWallRight = true;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                dx += xInput * moveSpeed * (Time.time - p.lastUpdateTime);
            }
        }
        else if(p.falling || p.jumping)
        {
            dx += xInput * airMoveSpeed * (Time.time - p.lastUpdateTime);
            if (xInput == 0)
            {
                dx *= (1 - airResistance);
                if(Mathf.Abs(dx) < 0.1f)
                {
                    dx = 0;
                }
            }
            if(touchingLeft && xInput < 0 
                && (!justLeftWallLeft || (justLeftWallLeft && lastXInput >= 0))) 
               //&& (lastXInput >= 0  || Mathf.Abs(rb.velocity.x) > 0.05f))
            {
                p.wallGrabbing = true;
                p.wallGrabLeft = true;
                justLeftWallLeft = false;
                justLeftWallRight = false;
                dx = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.gravityScale = wallGrabGravity;
                p.anim.StartWallGrab();
            }
            if (touchingRight && xInput > 0 
                && (!justLeftWallRight || (justLeftWallRight && lastXInput <= 0)))
                //&& (lastXInput <= 0 || Mathf.Abs(rb.velocity.x) > 0.05f))
            {
                p.wallGrabbing = true;
                p.wallGrabRight = true;
                justLeftWallRight = false;
                justLeftWallLeft = false;
                dx = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.gravityScale = wallGrabGravity;
                p.anim.StartWallGrab();
            }
        }
        else if (p.crouching)
        {
            dx = 0;
        }
        else if (p.shooting)
        {
            dx = 0;
        }
        else if (p.inHitStun)
        {
            dx = 0;
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
        if (xInput > 0 && !p.wallGrabbing) { p.anim.FaceRight(); }
        else if (xInput < 0 && !p.wallGrabbing) { p.anim.FaceLeft(); }
        dx = Mathf.Clamp(dx, -maxMoveSpeed, maxMoveSpeed);
        rb.velocity = new Vector2(dx, rb.velocity.y);
    }

    void UpdateVerticalMovement()
    {
        float dy = rb.velocity.y;

        if (p.crouching)
        {
            if (!p.controls.Intents.Contains(PlayerControls.IntentType.CROUCH))
            {
                p.crouching = false;
            }
            if (p.controls.Intents.Contains(PlayerControls.IntentType.CROUCH))
            {
                p.crouching = true;
            }
        }
        else if (p.jumping)
        {
            if(Time.time - jumpStartTime < jumpHoldLength)
            {
                if (p.controls.Intents.Contains(PlayerControls.IntentType.JUMP))
                {
                    dy = jumpMaxSpeed;
                }
            }
            else
            {
                p.jumping = false;
                if (!p.falling) p.anim.StartFalling();
                p.falling = true;
            }
        }
        else if (p.wallGrabbing)
        {
            if(p.wallGrabLeft && !touchingLeft)
            {
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                p.anim.StartFalling();
            }
            else if(p.wallGrabRight && !touchingRight)
            {
                p.wallGrabbing = false;
                p.wallGrabRight = false;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                p.anim.StartFalling();
            }
            else if (p.controls.Intents.Contains(PlayerControls.IntentType.JUMP) &&
                !p.controls.PreviousIntents.Contains(PlayerControls.IntentType.JUMP))
            {
                justLeftWallLeft = p.wallGrabLeft;
                justLeftWallRight = p.wallGrabRight;
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                p.wallGrabRight = false;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                p.jumping = true;
                dy = jumpMaxSpeed;
                p.anim.StartJump();
            }
            else if (p.controls.Intents.Contains(PlayerControls.IntentType.CROUCH) &&
                !p.controls.PreviousIntents.Contains(PlayerControls.IntentType.CROUCH))
            {
                justLeftWallLeft = p.wallGrabLeft;
                justLeftWallRight = p.wallGrabRight;
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                p.wallGrabRight = false;
                wallLeaveTime = Time.time;
                p.falling = true;
                rb.gravityScale = normalGravity;
                p.anim.StartFalling();
            }
            else
            {
                
            }
        }
        else if (p.falling)
        {
            if(p.controls.Intents.Contains(PlayerControls.IntentType.JUMP) && 
                !p.controls.PreviousIntents.Contains(PlayerControls.IntentType.JUMP) &&
                Time.time - wallLeaveTime < jumpGracePeriod)
            {
                p.jumping = true;
                jumpStartTime = Time.time;
                dy = jumpMaxSpeed;
                p.anim.StartJump();
            }
        }
        else if(!p.inHitStun)
        {
            if (touchingBottom && p.controls.Intents.Contains(PlayerControls.IntentType.JUMP) && 
                !p.controls.PreviousIntents.Contains(PlayerControls.IntentType.JUMP))
            {
                p.jumping = true;
                jumpStartTime = Time.time;
                dy = jumpMaxSpeed;
                p.anim.StartJump();
            }
            if (touchingBottom && p.controls.Intents.Contains(PlayerControls.IntentType.CROUCH) &&
                !p.controls.PreviousIntents.Contains(PlayerControls.IntentType.CROUCH))
            {
                p.crouching = true;
                p.anim.StartCrouch();
            }
            if(!touchingBottom && !touchingTop && !touchingLeft && !touchingRight)
            {
                if (!p.falling) p.anim.StartFalling();
                p.falling = true;
            }
        }
        dy = Mathf.Clamp(dy, -maxFallSpeed, jumpMaxSpeed);
        rb.velocity = new Vector2(rb.velocity.x, dy);
    }

    public void WasHit()
    {
        if (p.wallGrabbing)
        {
            if (p.wallGrabLeft )
            {
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                p.anim.StartFalling();
            }
            else if (p.wallGrabRight )
            {
                p.wallGrabbing = false;
                p.wallGrabRight = false;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                p.anim.StartFalling();
            }
        }

    }
    public void TriggerDeath()
    {
        if (p.wallGrabbing)
        {
            if (p.wallGrabLeft)
            {
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                p.anim.StartFalling();
            }
            else if (p.wallGrabRight)
            {
                p.wallGrabbing = false;
                p.wallGrabRight = false;
                wallLeaveTime = Time.time;
                rb.gravityScale = normalGravity;
                p.anim.StartFalling();
            }
        }
        p.crouching = false;
        p.jumping = false;
        p.falling = false;
        p.wallGrabbing = false;
        p.wallGrabRight = p.wallGrabLeft = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    ContactPoint2D[] cleanContactPoints(ContactPoint2D[] contacts)
    {
        List<ContactPoint2D> cleanContactList = new List<ContactPoint2D>();
        for(int i = 0; i < contacts.Length; i++)
        {
            bool hasDuplicate = false;
            for(int j = i + 1; j < contacts.Length; j++)
            {
                if(contacts[i].point == contacts[j].point)
                {
                    hasDuplicate = true;
                }
                if(Vector2.Distance(contacts[i].point, contacts[j].point) < 0.1f){
                    hasDuplicate = true;
                }
            }
            if (!hasDuplicate)
            {
                cleanContactList.Add(contacts[i]);
            }
        }
        return cleanContactList.ToArray();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contactPoints);
        contactPoints = cleanContactPoints(contactPoints);
        currentContactPoints = contactPoints;
        touchingBottom = isBottomCollision(contactPoints);
        touchingTop = isTopCollision(contactPoints);
        touchingLeft = isLeftCollision(contactPoints);
        touchingRight = isRightCollision(contactPoints);
        if (touchingBottom)
        {
            justLeftWallLeft = false;
            justLeftWallRight = false;
        }
        if (collision.transform.tag == "Map")
        {
            if(p.falling && touchingBottom)
            {
                p.falling = false;
            }
            if (p.jumping && touchingTop)
            {
                p.jumping = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                p.falling = true;
            }
            if(p.wallGrabbing && touchingBottom)
            {
                if (p.wallGrabLeft) p.anim.FaceRight();
                if (p.wallGrabRight) p.anim.FaceLeft();
                p.wallGrabbing = false;
                p.wallGrabLeft = false;
                p.wallGrabRight = false;
                rb.gravityScale = normalGravity;
            }
        }        
    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        currentContactPoints = new ContactPoint2D[0];
        touchingBottom = isBottomCollision(currentContactPoints);
        touchingTop = isTopCollision(currentContactPoints);
        touchingLeft = isLeftCollision(currentContactPoints);
        touchingRight = isRightCollision(currentContactPoints);
        
    }
    void printContacts(ContactPoint2D[] contacts)
    {
        for (int i = 0; i < contacts.Length; i++)
        {
            print(contacts[i].point);
        }
    }
    bool isBottomCollision(ContactPoint2D[] contacts)
    {
        int numBelow = 0;
        for(int i = 0; i < contacts.Length; i++)
        {
            if(contacts[i].point.y <= box.bounds.center.y - box.bounds.size.y / 2f)
            {
                numBelow++;
            }
        }
        return numBelow >= 2;
    }
    bool isTopCollision(ContactPoint2D[] contacts)
    {
        int numAbove = 0;
        for (int i = 0; i < contacts.Length; i++)
        {
            if (contacts[i].point.y >= box.bounds.center.y + box.bounds.size.y / 2f)
            {
                numAbove++;
            }
        }
        return numAbove >= 2;
    }
    bool isLeftCollision(ContactPoint2D[] contacts)
    {
        int numLeft = 0;
        for (int i = 0; i < contacts.Length; i++)
        {
            if (contacts[i].point.x <= box.bounds.center.x - box.bounds.size.x / 2f)
            {
                numLeft++;
            }
        }
        return numLeft >= 2;
    }
    bool isRightCollision(ContactPoint2D[] contacts)
    {
        int numRight = 0;
        for (int i = 0; i < contacts.Length; i++)
        {
            if (contacts[i].point.x >= box.bounds.center.x + box.bounds.size.x / 2f)
            {
                numRight++;
            }
        }
        return numRight >= 2;
    }
}
