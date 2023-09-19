using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/Seeker")]
public class Seeker : AttackEvent
{

    
    public float delay;
    public DelayedDangerZone attackEffect;
 
    
    public override void Fire()
    {
        if (attackEffect == null)
            attackEffect = Wobbit.instance.seekerTest;

        DelayedDangerZone b = (Instantiate(attackEffect, Wobbit.instance.player.position, Quaternion.identity));
        b.InitialiseOnTimer(0,BeatBroadcast.instance.beatLength + delay, BeatBroadcast.instance.beatLength);
      
    }

    public override void HookUp(EventEditor eventEditor)
    {
        ValueEditor valueEditor;

        //Set Up Beam Amount
        valueEditor = eventEditor.CreateEditor();
        valueEditor.SetListener((float f) => { delay = (int)f; }, delay, "Delay");
    }

}
