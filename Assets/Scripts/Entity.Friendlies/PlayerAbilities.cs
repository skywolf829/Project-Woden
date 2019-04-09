using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerV2))]
public class PlayerAbilities : MonoBehaviour
{
    PlayerV2 p;

    public GameObject Element1, Element2, Element3, Element4;
    public GameObject DefensiveElement1, DefensiveElement2, DefensiveElement3, DevensiveElement4;
    public GameObject ChargedElement1, ChargedElement2, ChargedElement3, ChargedElement4;
    public GameObject ChanneledElement1, ChanneledElement2, ChanneledElement3, ChanneledElement4;

    private void Awake()
    {
        p = GetComponent<PlayerV2>();
    }

    public IEnumerator UpdateAbilities()
    {

        yield return null;
    }
}
