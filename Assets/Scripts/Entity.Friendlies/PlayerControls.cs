using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerV2))]
public class PlayerControls : MonoBehaviour
{

    PlayerV2 p;
    public float movementDeadzone = 0.1f;
    public float crouchDeadzone = 0.9f;
    public float jumpDeadzone = 0.9f;
    public enum IntentType
    {
        LEFT, 
        RIGHT,
        JUMP,
        CROUCH,
        ELEMENT1,
        ELEMENT2,
        ELEMENT3,
        ELEMENT4,
        DEFENSEMODIFIER,
        CHANNELMODIFIER,
        CHARGEMODIFIER,
        PAUSE
    }

    public List<IntentType> Intents = new List<IntentType>();
    public List<IntentType> PreviousIntents = new List<IntentType>();
    List<IntentType> modifiersActive = new List<IntentType>();
    private void Awake()
    {
        p = GetComponent<PlayerV2>();
    }
    public IEnumerator UpdateControls()
    {
        PreviousIntents = new List<IntentType>();
        for(int i = 0; i < Intents.Count; i++)
        {
            PreviousIntents.Add(Intents[i]);
        }

        Intents = new List<IntentType>();
        if (Input.GetAxisRaw("Horizontal") < -movementDeadzone || Input.GetButton("HorizontalButtonLeft")) Intents.Add(IntentType.LEFT);
        if (Input.GetAxisRaw("Horizontal") > movementDeadzone || Input.GetButton("HorizontalButtonRight")) Intents.Add(IntentType.RIGHT);
        if (Input.GetAxisRaw("Vertical") > jumpDeadzone || Input.GetButton("VerticalButtonUp")) Intents.Add(IntentType.JUMP);
        if (Input.GetAxisRaw("Vertical") < -crouchDeadzone || Input.GetButton("VerticalButtonDown")) Intents.Add(IntentType.CROUCH);

        if ((Input.GetButton("DefenseModifierButton") || Input.GetAxisRaw("DefenseModifier") > 0) && !modifiersActive.Contains(IntentType.DEFENSEMODIFIER)) modifiersActive.Add(IntentType.DEFENSEMODIFIER);
        if (!(Input.GetButton("DefenseModifierButton") || Input.GetAxisRaw("DefenseModifier") > 0) && modifiersActive.Contains(IntentType.DEFENSEMODIFIER)) modifiersActive.Remove(IntentType.DEFENSEMODIFIER);
        if (Input.GetButton("ChannelModifier") && !modifiersActive.Contains(IntentType.CHANNELMODIFIER)) modifiersActive.Add(IntentType.CHANNELMODIFIER);
        if (!Input.GetButton("ChannelModifier") && modifiersActive.Contains(IntentType.CHANNELMODIFIER)) modifiersActive.Remove(IntentType.CHANNELMODIFIER);
        if ((Input.GetButton("ChargeModifierButton") || Input.GetAxisRaw("ChargeModifier") > 0)  && !modifiersActive.Contains(IntentType.CHARGEMODIFIER)) modifiersActive.Add(IntentType.CHARGEMODIFIER);
        if (!(Input.GetButton("ChargeModifierButton") || Input.GetAxisRaw("ChargeModifier") > 0) && modifiersActive.Contains(IntentType.CHARGEMODIFIER)) modifiersActive.Remove(IntentType.CHARGEMODIFIER);

        if(modifiersActive.Count > 0) Intents.Add(modifiersActive[modifiersActive.Count - 1]);
        if (Input.GetButton("Element1")) Intents.Add(IntentType.ELEMENT1);
        if (Input.GetButton("Element2")) Intents.Add(IntentType.ELEMENT2);
        if (Input.GetButton("Element3")) Intents.Add(IntentType.ELEMENT3);
        if (Input.GetButton("Element4")) Intents.Add(IntentType.ELEMENT4);
        if (Input.GetButton("Pause")) Intents.Add(IntentType.PAUSE);
        yield return null;
    }
}
