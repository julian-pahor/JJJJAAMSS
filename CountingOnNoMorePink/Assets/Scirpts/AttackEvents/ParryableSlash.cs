using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/ParryableSlash")]
public class ParryableSlash : AttackEvent
{
    public float angle;
    public float distance;
    public float time;
    public float curve;
    public bool reverse;
    public override void Fire()
    {

        Transform boss1 = Wobbit.instance.hand1;
        Transform boss2 = Wobbit.instance.hand2;
        Transform player = Wobbit.instance.player;

        Vector3 pos1 = reverse ? boss2.position : boss1.position;
        Vector3 pos2 = reverse ? boss1.position : boss2.position;

        Tracer t = Instantiate(Wobbit.instance.tracer, pos1, Quaternion.identity);

    
        t.SetUp(pos1, pos2, player,curve, time);
        
    }


}
