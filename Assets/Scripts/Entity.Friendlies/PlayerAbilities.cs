using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerV2))]
public class PlayerAbilities : MonoBehaviour
{
    PlayerV2 p;

    private void Awake()
    {
        p = GetComponent<PlayerV2>();
    }
    List<GameObject> abilities;
    List<GameObject> specialAbilities;
}
