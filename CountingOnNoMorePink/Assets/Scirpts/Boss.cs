using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
   
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Wobbit.instance.player.position- transform.position);
    }
}
