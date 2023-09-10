using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/Seeker")]
public class Seeker : AttackEvent
{

    //public float safeTime;
    //public float height;
 
    
    public override void Fire()
    {
        

        //Vector3 pos = Wobbit.instance.player.position + (Vector3.up * height);

        //GameObject go = Instantiate(Wobbit.instance.warning, pos, Quaternion.Euler(new Vector3(-90f,0f,0)));
        DelayedDangerZone b = (Instantiate(Wobbit.instance.delayedDangerZoneTest, Wobbit.instance.player.position, Quaternion.identity));
        b.InitialiseOnBeat(1,1);
       // go.transform.SetParent(b.transform);
    }

    public override void HookUp(EventEditor ee)
    {
        base.HookUp(ee);
    }

}
