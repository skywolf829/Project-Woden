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

    List<string> abilityHistory;

    float abilityReleaseTime = 0;
    public bool secondAttackQueued, thirdAttackQueued;

    int elementToShoot = 0;
    int blinkTeleportDir = 0;
    private void Awake()
    {
        p = GetComponent<PlayerV2>();
        abilityHistory = new List<string>();
    }

    public IEnumerator UpdateAbilities()
    {
        p.canUseAbility = abilityReleaseTime <= Time.time;
        if(!secondAttackQueued && !thirdAttackQueued)
        {
            p.shooting = abilityReleaseTime > Time.time;
        }
        if (!p.canUseAbility)
        {
            if (p.controls.Intents.Contains(PlayerControls.IntentType.ELEMENT1))
            {
                if (p.anim.inShot2Window && !secondAttackQueued)
                {
                    secondAttackQueued = true;
                    thirdAttackQueued = false;
                    p.anim.StartShooting2();
                    elementToShoot = 1;
                }
                else if (p.anim.inShot3Window && !thirdAttackQueued)
                {
                    secondAttackQueued = false;
                    thirdAttackQueued = true;
                    p.anim.StartShooting3();
                    elementToShoot = 1;
                }
            }
            else if (p.controls.Intents.Contains(PlayerControls.IntentType.ELEMENT2))
            {
                if (p.anim.inShot2Window && !secondAttackQueued)
                {
                    secondAttackQueued = true;
                    thirdAttackQueued = false;
                    p.anim.StartShooting2();
                    elementToShoot = 2;
                }
                else if (p.anim.inShot3Window && !thirdAttackQueued)
                {
                    secondAttackQueued = false;
                    thirdAttackQueued = true;
                    p.anim.StartShooting3();
                    elementToShoot = 2;
                }
            }
        }
        
        if (!p.falling && !p.jumping && p.canUseAbility)
        {
            if (p.controls.Intents.Contains(PlayerControls.IntentType.ELEMENT1))
            {
                if (p.controls.Intents.Contains(PlayerControls.IntentType.CHARGEMODIFIER))
                {
                    p.anim.StartChargedAttack();
                    elementToShoot = 1;
                    secondAttackQueued = thirdAttackQueued = false;
                }
                else
                {
                    secondAttackQueued = thirdAttackQueued = false;
                    p.anim.StartShooting();
                    elementToShoot = 1;
                }
            }
            else if (p.controls.Intents.Contains(PlayerControls.IntentType.ELEMENT2))
            {
                if (p.controls.Intents.Contains(PlayerControls.IntentType.CHARGEMODIFIER))
                {
                    p.anim.StartChargedAttack();
                    elementToShoot = 2;
                    secondAttackQueued = thirdAttackQueued = false;
                }
                else
                {
                    p.anim.StartShooting();
                    elementToShoot = 2;
                    secondAttackQueued = thirdAttackQueued = false;
                }
            }
            else if (p.controls.Intents.Contains(PlayerControls.IntentType.ELEMENT3))
            {
                if (p.controls.Intents.Contains(PlayerControls.IntentType.CHARGEMODIFIER))
                {
                    p.anim.StartChargedAttack();
                    elementToShoot = 3;
                    secondAttackQueued = thirdAttackQueued = false;
                }
                else
                {
                    p.anim.StartShooting();
                    elementToShoot = 3;
                    secondAttackQueued = thirdAttackQueued = false;
                }
            }
            else if (p.controls.Intents.Contains(PlayerControls.IntentType.ELEMENT4))
            {
                if (p.controls.Intents.Contains(PlayerControls.IntentType.CHARGEMODIFIER))
                {
                    p.anim.StartChargedAttack();
                    elementToShoot = 4;
                    secondAttackQueued = thirdAttackQueued = false;
                }
                else
                {
                    p.anim.StartShooting();
                    elementToShoot = 4;
                    secondAttackQueued = thirdAttackQueued = false;
                }
            }
            else
            {
                secondAttackQueued = thirdAttackQueued = false;
            }
            if (p.controls.Intents.Contains(PlayerControls.IntentType.CHANNELMODIFIER))
            {
                p.anim.StartBlink();
            }
        }

        yield return null;
    }
    public void OnBlinkStart()
    {
        p.blinking = true;
        p.invincible = true;
        int xInput = 0;
        xInput += p.controls.Intents.Contains(PlayerControls.IntentType.RIGHT) ? 1 : 0;
        xInput -= p.controls.Intents.Contains(PlayerControls.IntentType.LEFT) ? 1 : 0;
        blinkTeleportDir = xInput;
    }
    public void OnBlinkEnd()
    {
        p.blinking = false;
        p.invincible = false;
    }
    public void DoBlinkTeleport()
    {
        transform.position += blinkTeleportDir * new Vector3(2, 0);
    }
    public void SetShootReleaseTime()
    {
        AnimatorClipInfo[] clipInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        abilityReleaseTime = Time.time + clipInfo[0].clip.length;
        p.shooting = true;
    }
    public void CastElement(int num)
    {
        CastElement(num, false);
    }
    public void CastElementCharged()
    {
        CastElement(0, true);
    }
    public void CastElement(int num = 0, bool charged = false)
    {
        GameObject g = null;
        if (elementToShoot == 1)
            g = Instantiate(Element1);        
        else if (elementToShoot == 2)
            g = Instantiate(Element2);
        else if (elementToShoot == 3)
            g = Instantiate(Element3);
        else if (elementToShoot == 4)
            g = Instantiate(Element4);
        else
        {
            print("Error in cast element");
            return;
        }
        g.transform.position = transform.position + (p.transform.localScale.x > 0 ? new Vector3(2, 0) : new Vector3 (-2, 0));
        g.SendMessage("SetAttack", num);
        
        p.canUseAbility = false;
        secondAttackQueued = thirdAttackQueued = false;
        abilityHistory.Add(g.name);
        if(abilityHistory.Count > 20)
        {
            abilityHistory.RemoveAt(0);
        }
    }

    public void TriggerDeath()
    {
        p.shooting = false;
        p.wallShooting = false;
        secondAttackQueued = false;
        thirdAttackQueued = false;
        p.blinking = false;
        p.invincible = false;
        p.canUseAbility = false;
    }

}
