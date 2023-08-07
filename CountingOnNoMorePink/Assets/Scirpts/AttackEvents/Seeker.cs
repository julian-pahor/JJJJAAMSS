using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/Seeker")]
public class Seeker : AttackEvent
{

    public float safeTime;

    public override void Fire()
    {
        BoomBlock b = (Instantiate(Wobbit.instance.zoneFab, Wobbit.instance.player.position, Quaternion.identity));
        b.Initialise(safeTime);
    }


}
