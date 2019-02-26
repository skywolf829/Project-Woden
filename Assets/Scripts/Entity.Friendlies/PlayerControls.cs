using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerV2))]
public class PlayerControls : MonoBehaviour
{

    PlayerV2 p;

    public KeyCode leftKey, rightKey;
    public KeyCode jumpKey;
    public KeyCode crouchKey;
    public KeyCode ability1Key, ability2Key, ability3Key, ability4Key;
    public KeyCode specialAbilityModifierKey;

    public enum IntentType
    {
        LEFT, 
        RIGHT,
        JUMP,
        CROUCH,
        ABILITY1,
        ABILITY2,
        ABILITY3,
        ABILITY4,
        SPECIALABILITY1,
        SPECIALABILITY2,
        SPECIALABILITY3,
        SPECIALABILITY4
    }

    public List<IntentType> Intents = new List<IntentType>();

    private void Awake()
    {
        p = GetComponent<PlayerV2>();
    }
    public IEnumerator UpdateControls()
    {
        Intents = new List<IntentType>();

        if (Input.GetKey(leftKey)) Intents.Add(IntentType.LEFT);
        if (Input.GetKey(rightKey)) Intents.Add(IntentType.RIGHT);
        if (Input.GetKey(jumpKey)) Intents.Add(IntentType.JUMP);
        if (Input.GetKey(crouchKey)) Intents.Add(IntentType.CROUCH);
        if (Input.GetKey(specialAbilityModifierKey))
        {
            if (Input.GetKey(ability1Key)) Intents.Add(IntentType.SPECIALABILITY1);
            if (Input.GetKey(ability2Key)) Intents.Add(IntentType.SPECIALABILITY2);
            if (Input.GetKey(ability3Key)) Intents.Add(IntentType.SPECIALABILITY3);
            if (Input.GetKey(ability4Key)) Intents.Add(IntentType.SPECIALABILITY4);
        }
        else
        {
            if (Input.GetKey(ability1Key)) Intents.Add(IntentType.ABILITY1);
            if (Input.GetKey(ability2Key)) Intents.Add(IntentType.ABILITY2);
            if (Input.GetKey(ability3Key)) Intents.Add(IntentType.ABILITY3);
            if (Input.GetKey(ability4Key)) Intents.Add(IntentType.ABILITY4);
        }

        yield return null;
    }
}
