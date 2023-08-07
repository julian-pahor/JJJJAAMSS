using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/Seeker")]
public class Seeker : AttackEvent
{

    public float safeTime;
 

    public override void Fire()
    {
        Instantiate(Wobbit.instance.warning, Wobbit.instance.player.position, Quaternion.Euler(new Vector3(-90f,0f,0)));
        BoomBlock b = (Instantiate(Wobbit.instance.zoneFab, Wobbit.instance.player.position, Quaternion.identity));
        b.Initialise(safeTime);
    }


}
