﻿using UnityEngine;
using System.Collections;

public class FrostBlastScript : Projectile {

    public AudioClip persistantClip, explosionClip;
    AudioSource audioSource;

	void Awake()
    {
        damage = 35;
        moveSpeed = 7.5f;
        dissipateSpeed = 0.17f;
        description = "Ball of ice";
        type = "ice";
    }
    
}

