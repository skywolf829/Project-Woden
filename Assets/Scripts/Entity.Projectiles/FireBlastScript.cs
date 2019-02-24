﻿using UnityEngine;
using System.Collections;

public class FireBlastScript : Projectile {

    public AudioClip persistantClip, explosionClip;
    AudioSource audioSource;
    void Awake()
    {
        damage = 100;
        moveSpeed = 7.5f;
        dissipateSpeed = 0.15f;
        description = "Ball of fire";
        type = "fire";
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 10;
        audioSource.spatialize = true;
        audioSource.SetCustomCurve(AudioSourceCurveType.SpatialBlend,
            AnimationCurve.Linear(0, 0, audioSource.maxDistance, 1));
        audioSource.clip = persistantClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.Play();

    }
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        base.OnTriggerEnter2D(c);
        if(hit)
        {
            audioSource.Stop();
            audioSource.clip = explosionClip;
            audioSource.loop = false;
            audioSource.time = 0;
            audioSource.Play();
        }
    }
}

