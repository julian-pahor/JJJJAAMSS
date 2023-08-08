using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/Seeker")]
public class Seeker : AttackEvent
{

    public float safeTime;
    public float height;
 

    public override void Fire()
    {
        

        Vector3 pos = Wobbit.instance.player.position + (Vector3.up * height);

        //GameObject go = Instantiate(Wobbit.instance.warning, pos, Quaternion.Euler(new Vector3(-90f,0f,0)));
        BoomBlock b = (Instantiate(Wobbit.instance.zoneFab, Wobbit.instance.player.position, Quaternion.identity));
        b.Initialise(safeTime,pos, Wobbit.instance.player.position);
       // go.transform.SetParent(b.transform);
    }


}
