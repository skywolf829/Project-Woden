using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerV2))]
public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerV2 p;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        p = GetComponent<PlayerV2>();
    }
    public IEnumerator UpdateAnimation()
    {

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
