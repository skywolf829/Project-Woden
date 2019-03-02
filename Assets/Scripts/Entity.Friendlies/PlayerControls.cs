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

    [SerializeField]
    public IntentType XboxAButton;
    [SerializeField]
    public IntentType XboxBButton;
    [SerializeField]
    public IntentType XboxXButton;
    [SerializeField]
    public IntentType XboxYButton;
    [SerializeField]
    public IntentType XboxRightBumper;
    [SerializeField]
    public IntentType XboxLeftBumper;
    [SerializeField]
    public IntentType XboxRightTrigger;
    [SerializeField]
    public IntentType XboxLeftTrigger;
    [SerializeField]
    public IntentType XboxLeftStickXPositive;
    [SerializeField]
    public IntentType XboxLeftStickXNegative;
    [SerializeField]
    public IntentType XboxLeftStickYPositive;
    [SerializeField]
    public IntentType XboxLeftStickYNegative;
    [SerializeField]
    public IntentType XboxRightStickXPositive;
    [SerializeField]
    public IntentType XboxRightStickXNegative;
    [SerializeField]
    public IntentType XboxRightStickYPositive;
    [SerializeField]
    public IntentType XboxRightStickYNegative;
    [SerializeField]
    public IntentType XboxDPADYPositive;
    [SerializeField]
    public IntentType XboxDPADYNegative;
    [SerializeField]
    public IntentType XboxDPADXPositive;
    [SerializeField]
    public IntentType XboxDPADXNegative;
    [SerializeField]
    public IntentType XboxStartButton;
    [SerializeField]
    public IntentType XboxSelectButtom;
    [SerializeField]
    public IntentType XboxRightStickIn;
    [SerializeField]
    public IntentType XboxLeftStickIn;

    [SerializeField]
    public KeyCode XboxAButtonKeyboardMapping;
    [SerializeField]
    public KeyCode XboxBButtonKeyboardMapping;
    [SerializeField]
    public KeyCode XboxXButtonKeyboardMapping;
    [SerializeField]
    public KeyCode XboxYButtonKeyboardMapping;
    [SerializeField]
    public KeyCode XboxRightBumperKeyboardMapping;
    [SerializeField]
    public KeyCode XboxLeftBumperKeyboardMapping;
    [SerializeField]
    public KeyCode XboxRightTriggerKeyboardMapping;
    [SerializeField]
    public KeyCode XboxLeftTriggerKeyboardMapping;
    [SerializeField]
    public KeyCode XboxLeftStickXPositiveKeyboardMapping;
    [SerializeField]
    public KeyCode XboxLeftStickXNegativeKeyboardMapping;
    [SerializeField]
    public KeyCode XboxLeftStickYPositiveKeyboardMapping;
    [SerializeField]
    public KeyCode XboxLeftStickYNegativeKeyboardMapping;
    [SerializeField]
    public KeyCode XboxRightStickXPositiveKeyboardMapping;
    [SerializeField]
    public KeyCode XboxRightStickXNegativeKeyboardMapping;
    [SerializeField]
    public KeyCode XboxRightStickYPositiveKeyboardMapping;
    [SerializeField]
    public KeyCode XboxRightStickYNegativeKeyboardMapping;
    [SerializeField]
    public KeyCode XboxDPADXPositiveKeyboardMapping;
    [SerializeField]
    public KeyCode XboxDPADXNegativeKeyboardMapping;
    [SerializeField]
    public KeyCode XboxDPADYPositiveKeyboardMapping;
    [SerializeField]
    public KeyCode XboxDPADYNegativeKeyboardMapping;
    [SerializeField]
    public KeyCode XboxStartButtonKeyboardMapping;
    [SerializeField]
    public KeyCode XboxSelectButtonKeyboardMapping;
    [SerializeField]
    public KeyCode XboxRightStickInKeyboardMapping;
    [SerializeField]
    public KeyCode XboxLeftStickInKeyboardMapping;



    public enum IntentType
    {
        NONE,
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


        if ((Input.GetAxisRaw("XboxLeftStickHorizontal") > movementDeadzone || (XboxLeftStickXPositiveKeyboardMapping != KeyCode.None && Input.GetKey(XboxLeftStickXPositiveKeyboardMapping))) && XboxLeftStickXPositive != IntentType.NONE)
            Intents.Add(XboxLeftStickXPositive);
        if ((Input.GetAxisRaw("XboxLeftStickHorizontal") < -movementDeadzone || (XboxLeftStickXNegativeKeyboardMapping != KeyCode.None && Input.GetKey(XboxLeftStickXNegativeKeyboardMapping))) && XboxLeftStickXNegative != IntentType.NONE)
            Intents.Add(XboxLeftStickXNegative);
        if ((Input.GetAxisRaw("XboxLeftStickVertical") > jumpDeadzone || (XboxLeftStickYPositiveKeyboardMapping != KeyCode.None && Input.GetKey(XboxLeftStickYPositiveKeyboardMapping))) && XboxLeftStickYPositive != IntentType.NONE)
            Intents.Add(XboxLeftStickYPositive);
        if ((Input.GetAxisRaw("XboxLeftStickVertical") < -crouchDeadzone || (XboxLeftStickYNegativeKeyboardMapping != KeyCode.None && Input.GetKey(XboxLeftStickYNegativeKeyboardMapping))) && XboxLeftStickYNegative != IntentType.NONE)
            Intents.Add(XboxLeftStickYNegative);

        if ((Input.GetAxisRaw("XboxRightStickHorizontal") > 0.1f || (XboxRightStickXPositiveKeyboardMapping != KeyCode.None && Input.GetKey(XboxRightStickXPositiveKeyboardMapping))) && XboxRightStickXPositive != IntentType.NONE)
            Intents.Add(XboxRightStickXPositive);
        if ((Input.GetAxisRaw("XboxRightStickHorizontal") < -0.1f || (XboxRightStickXNegativeKeyboardMapping != KeyCode.None && Input.GetKey(XboxRightStickXNegativeKeyboardMapping))) && XboxRightStickXNegative != IntentType.NONE)
            Intents.Add(XboxRightStickXNegative);
        if ((Input.GetAxisRaw("XboxRightStickVertical") > 0.1f || (XboxRightStickYPositiveKeyboardMapping != KeyCode.None && Input.GetKey(XboxRightStickYPositiveKeyboardMapping))) && XboxRightStickYPositive != IntentType.NONE)
            Intents.Add(XboxRightStickYPositive);
        if ((Input.GetAxisRaw("XboxRightStickVertical") < -0.1f || (XboxRightStickYNegativeKeyboardMapping != KeyCode.None && Input.GetKey(XboxRightStickYNegativeKeyboardMapping))) && XboxRightStickYNegative != IntentType.NONE)
            Intents.Add(XboxRightStickYNegative);

        if ((Input.GetAxisRaw("XboxDPADHorizontal") > 0.1f || (XboxDPADXPositiveKeyboardMapping != KeyCode.None && Input.GetKey(XboxDPADXPositiveKeyboardMapping))) && XboxDPADXPositive != IntentType.NONE)
            Intents.Add(XboxDPADXPositive);
        if ((Input.GetAxisRaw("XboxDPADHorizontal") < -0.1f || (XboxDPADXNegativeKeyboardMapping != KeyCode.None && Input.GetKey(XboxDPADXNegativeKeyboardMapping))) && XboxDPADXNegative != IntentType.NONE)
            Intents.Add(XboxDPADXNegative);
        if ((Input.GetAxisRaw("XboxDPADVertical") > 0.1f || (XboxDPADYPositiveKeyboardMapping != KeyCode.None && Input.GetKey(XboxDPADYPositiveKeyboardMapping))) && XboxDPADYPositive != IntentType.NONE)
            Intents.Add(XboxDPADYPositive);
        if ((Input.GetAxisRaw("XboxDPADVertical") < -0.1f || (XboxDPADYNegativeKeyboardMapping != KeyCode.None && Input.GetKey(XboxDPADYNegativeKeyboardMapping))) && XboxDPADYNegative != IntentType.NONE)
            Intents.Add(XboxDPADYNegative);



        if ((Input.GetButton("XboxLeftBumper") || (XboxLeftBumperKeyboardMapping != KeyCode.None &&  Input.GetKey(XboxLeftBumperKeyboardMapping))) && XboxLeftBumper != IntentType.NONE)
            Intents.Add(XboxLeftBumper);
        if ((Input.GetButton("XboxRightBumper") || (XboxRightBumperKeyboardMapping != KeyCode.None &&  Input.GetKey(XboxRightBumperKeyboardMapping))) && XboxRightBumper != IntentType.NONE)
            Intents.Add(XboxRightBumper);
        if ((Input.GetAxisRaw("XboxRightTrigger") > 0.1f|| (XboxRightTriggerKeyboardMapping != KeyCode.None &&  Input.GetKey(XboxRightTriggerKeyboardMapping))) && XboxRightTrigger != IntentType.NONE)
            Intents.Add(XboxRightTrigger);
        if ((Input.GetAxisRaw("XboxLeftTrigger") > 0.1f || (XboxLeftTriggerKeyboardMapping != KeyCode.None &&  Input.GetKey(XboxLeftTriggerKeyboardMapping))) && XboxLeftTrigger != IntentType.NONE)
            Intents.Add(XboxLeftTrigger);

        if ((Input.GetButton("XboxA") || (XboxAButtonKeyboardMapping != KeyCode.None && Input.GetKey(XboxAButtonKeyboardMapping))) && XboxAButton != IntentType.NONE)
            Intents.Add(XboxAButton);
        if ((Input.GetButton("XboxB") || (XboxBButtonKeyboardMapping != KeyCode.None && Input.GetKey(XboxBButtonKeyboardMapping))) && XboxBButton != IntentType.NONE)
            Intents.Add(XboxBButton);
        if ((Input.GetButton("XboxX") || (XboxXButtonKeyboardMapping != KeyCode.None && Input.GetKey(XboxXButtonKeyboardMapping))) && XboxXButton != IntentType.NONE)
            Intents.Add(XboxXButton);
        if ((Input.GetButton("XboxY") || (XboxYButtonKeyboardMapping != KeyCode.None && Input.GetKey(XboxYButtonKeyboardMapping))) && XboxYButton != IntentType.NONE)
            Intents.Add(XboxYButton);

        if ((Input.GetButton("XboxRightStickIn") || (XboxRightStickInKeyboardMapping != KeyCode.None && Input.GetKey(XboxRightStickInKeyboardMapping))) && XboxRightStickIn != IntentType.NONE)
            Intents.Add(XboxRightStickIn);
        if ((Input.GetButton("XboxLeftStickIn") || (XboxLeftStickInKeyboardMapping != KeyCode.None && Input.GetKey(XboxLeftStickInKeyboardMapping))) && XboxLeftStickIn != IntentType.NONE)
            Intents.Add(XboxLeftStickIn);

        if ((Input.GetButton("XboxStartButton") || (XboxStartButtonKeyboardMapping != KeyCode.None && Input.GetKey(XboxStartButtonKeyboardMapping))) && XboxStartButton != IntentType.NONE)
            Intents.Add(XboxStartButton);
        if ((Input.GetButton("XboxSelectButton") || (XboxSelectButtonKeyboardMapping != KeyCode.None && Input.GetKey(XboxSelectButtonKeyboardMapping))) && XboxSelectButtom != IntentType.NONE)
            Intents.Add(XboxSelectButtom);
        yield return null;
    }
}
