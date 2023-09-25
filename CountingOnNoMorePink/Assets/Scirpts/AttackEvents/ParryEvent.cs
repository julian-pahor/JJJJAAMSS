using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/ParryEvent")]
public class ParryEvent : AttackEvent
{
    public ParryType type = ParryType.ThreeBeat;
    public enum ParryType {OneBeat, TwoBeat, ThreeBeat}

    public override void Arm(int beatIndex)
    {
        if(beatIndex == firingIndex - ((int)type))
        {
            //Call corresponding fmod event
            switch(type)
            {
                case ParryType.OneBeat:

                    break;
                case ParryType.TwoBeat:

                    break;
                case ParryType.ThreeBeat:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Attacks/DemoAttack", Wobbit.instance.bossOrigin.position);
                    break;

            }
        }
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
