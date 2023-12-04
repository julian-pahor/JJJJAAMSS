using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/ParryEvent")]
public class ParryEvent : AttackEvent
{
    public ParryType type = ParryType.TwoBeat;
    public enum ParryType {OneBeat, TwoBeat, ThreeBeat}

    public override void Arm(int beatIndex)
    {
        if(beatIndex == firingIndex - ((int)type) - 1)
        {
            //Call corresponding fmod event
            switch(type)
            {
                case ParryType.OneBeat:

                    break;
                case ParryType.TwoBeat:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Attacks/DemoAttack", Wobbit.instance.bossOrigin.position);
                    Instantiate(Wobbit.instance.pa2);
                    AnimationManager.instance.FireAttackAnim();

                    //create indicator PLACEHOLDER - this needs to be done dynamically and passed the number of beats before the parry attack lands
                    Wobbit.instance.CreateCountDownIndicator(3);

                    break;
                case ParryType.ThreeBeat:
                    
                    break;
            }
        }
    }


    public override void FirePreview()
    {
        //DO NOTHING
    }

    public override void Fire()
    {
        //Compare/look for player input
        Wobbit.instance.playerParry.ParryChance();
    }

    public override void HookUp(EventEditor ee)
    {
        
    }
}
