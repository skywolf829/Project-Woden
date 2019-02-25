using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerV2))]
public class PlayerControls : MonoBehaviour
{
    PlayerV2 p;

    private void Awake()
    {
        p = GetComponent<PlayerV2>();
    }
    public IEnumerator UpdateControls()
    {

        yield return null;
    }
}
