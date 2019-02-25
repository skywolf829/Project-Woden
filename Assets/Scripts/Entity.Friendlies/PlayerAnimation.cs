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
}
